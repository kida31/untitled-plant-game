using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.NPC.NpcTask;
using untitledplantgame.NPC.NpcType;
using untitledplantgame.NPC.RoutinePlanner;

namespace untitledplantgame.NPC.Routine;

/// <summary>
///		An Npc Routine is supposed to be a collection of one or more NpcTasks.
///		A Routine will always execute all Tasks it has been given, but will only execute one Task at a time.
///		However, there are two rules that need to be strictly followed to make a Routine and RoutinePlanner work.
///
///		Rule 1: A RoutinePlanner has to be the child of a Npc
///		Rule 2: All Routines are children of RoutinePlanners
/// </summary>
public partial class NpcRoutine : Node
{
	public enum Options { TimeOfDay, PlayerInteraction }
	[Export] public Options RoutineTrigger { get; set; } = Options.TimeOfDay;
	
	[Export(PropertyHint.Range, "0,23")]
	public byte RoutineStartHours { get; set; } = 7;

	[Export(PropertyHint.Range, "0,59")]
	public byte RoutineStartMinutes { get; set; } = 30;

	[Export] public NpcRoutine NextRoutine;
	
	private bool _correctTimeOfDay;
	private bool _playerInteracted;
	private bool _previousRoutineFinished;
	private bool _isStartingRoutine;
	private bool _playerInterruption;
	private NpcRoutinePlanner _owningRoutinePlanner;
	private List<INpcTask> _npcTasks;
	private event EventHandler PlayerInterruptedRoutine;
	private event EventHandler PreviousRoutineFinishedTask;
	private event EventHandler RightTimeOfDayReached;
	private event EventHandler PlayerInteracted;
	
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);

		/*
		 * Rule: A RoutinePlanner has to be the child of a Npc
		 */
		_owningRoutinePlanner = (NpcRoutinePlanner) GetParent();
		
		if (RoutineTrigger == Options.TimeOfDay)
		{
			_logger.Debug("The time of day to trigger this routine is: " + RoutineStartHours + "h : " + RoutineStartMinutes + "min.");
			ClockBasedEventController.Instance.AddClockBasedEvent(
				RoutineStartHours, 
				RoutineStartMinutes, 
				TimeToTriggerRoutine
				);
		}
		else
		{
			_logger.Debug("This routine will trigger once the player interacts with the Npc.");
			/*
			 * Rule: All Routines are children of RoutinePlanners
			 */
			var npc = (StandardNpc) _owningRoutinePlanner.GetParent(); 
			npc.AssignMethodToInteractionEvent(TriggerPlayerRoutineAfterInteraction);
		}
		
		_npcTasks = new List<INpcTask>();
		
		foreach (var node in GetChildren())
		{
			if (node is INpcTask npcTask)
			{
				_npcTasks.Add(npcTask);
			}
		}
	}

	public void InitializeRoutine(NpcRoutinePlanner npcRoutinePlanner)
	{
		_owningRoutinePlanner = npcRoutinePlanner;
	}
	
	public void MakeThisRoutineTheStartingPoint()
	{
		_isStartingRoutine = true;
	}
	
	public async Task ExecuteAllTasks()
	{
		_logger.Debug("Starting to execute all tasks in this routine.");
		await Task.Yield();
		
		if (RoutineTrigger == Options.TimeOfDay)
		{
			await WaitUntilCorrectTimeOfDay();
		}
		else 
		{
			await WaitUntilPlayerInteracted();
		}
		
		foreach (var npcTask in _npcTasks)
		{
			_owningRoutinePlanner.ActiveTask = npcTask;
			npcTask.InitializeTask(_owningRoutinePlanner.GetNpcExecutingRoutines());
			await npcTask.ExecuteNpcTask();
		}
		
		// After we started the routine, we can be sure we don't start it again immediately!
		_correctTimeOfDay = false;
		_playerInteracted = false;
		_owningRoutinePlanner.ActiveTask = null;
		
		NextRoutine?.PreviousRoutineFinishTask(); // The next Routine can be null. Godot won't catch the exception ⇒ NPC is stuck
	}
	
	//------------------------------------------------------------------------------------------------------------------------------------//
	private async void TimeToTriggerRoutine()
	{
		await Task.Yield();
		await Task.Delay(1);
		
		if (!_isStartingRoutine)
		{
			await WaitUntilPreviousRoutineFinished();
			_owningRoutinePlanner.LastRoutine = this;
			
			_logger.Debug(Name + " is waiting for the Player to finish the Dialogue.");
			await WaitUntilPlayerFinishedInterruption(); // No need to account for interruption WITHIN tasks, is handled elsewhere
		}
		else
		{
			_owningRoutinePlanner.LastRoutine = this;
			
			_logger.Debug(Name + " is waiting for the Player to finish the Dialogue.");
			await WaitUntilPlayerFinishedInterruption(); // No need to account for interruption WITHIN tasks, is handled elsewhere
		}

		_correctTimeOfDay = true;
		_previousRoutineFinished = false;
		
		RightTimeOfDayReached?.Invoke(this, EventArgs.Empty);
	}
	//------------------------------------------------------------------------------------------------------------------------------------//
	
	
	/// <summary>
	///		Scenario 1:
	///		Handles the scenario when a Routine is based on a specific time of day
	/// </summary>
	private Task WaitUntilCorrectTimeOfDay()
	{
		var tcs = new TaskCompletionSource<bool>();
		
		EventHandler onConditionMet = null;
		onConditionMet = (sender, args) =>
		{
			if (!_correctTimeOfDay)
			{
				return;
			}
			
			tcs.TrySetResult(true);
			RightTimeOfDayReached -= onConditionMet;
		};
		
		RightTimeOfDayReached += onConditionMet;
		
		return tcs.Task;
	}
	//------------------------------------------------------------------------------------------------------------------------------------//
	
	
	/// <summary>
	///		Scenario 2:
	///		Handles the scenario when a player is necessary to progress the Routine with an interaction
	/// </summary>
	private Task WaitUntilPlayerInteracted()
	{
		var tcs = new TaskCompletionSource<bool>();
		
		EventHandler onConditionMet = null;
		onConditionMet = (sender, args) =>
		{
			if (!_playerInteracted)
			{
				return;
			}
			
			tcs.TrySetResult(true);
			PlayerInteracted -= onConditionMet;
		};
		
		PlayerInteracted += onConditionMet;

		return tcs.Task;
	}
	
	private void TriggerPlayerRoutineAfterInteraction()
	{
		_playerInteracted = true;
		PlayerInteracted?.Invoke(this, EventArgs.Empty);
	}
	//------------------------------------------------------------------------------------------------------------------------------------//
	
	
	/// <summary>
	///		Scenario 3:
	///		Handles the scenario where a routine doesn't finish in time, causing the next routine to wait until the previous one is finished
	/// </summary>
	private Task WaitUntilPreviousRoutineFinished()
	{
		var tcs = new TaskCompletionSource<bool>();
		
		EventHandler onConditionMet = null;
		onConditionMet = (sender, args) =>
		{
			if (!_previousRoutineFinished)
			{
				return;
			}
			tcs.TrySetResult(true);
			PreviousRoutineFinishedTask -= onConditionMet;
		};
		
		PreviousRoutineFinishedTask += onConditionMet;
		
		if (_previousRoutineFinished)
		{
			PreviousRoutineFinishedTask?.Invoke(this, EventArgs.Empty);
		}
		
		return tcs.Task;
	}
	
	private void PreviousRoutineFinishTask()
	{
		_logger.Debug("The previous Routine has been finished.");
		_previousRoutineFinished = true;
		PreviousRoutineFinishedTask?.Invoke(this, EventArgs.Empty);
	}
	//------------------------------------------------------------------------------------------------------------------------------------//
	
	
	/// <summary>
	///		Scenario 4:
	///		A Player interrupt the NPC during downtime with no active Routine. The start of further routines needs to be postponed.
	/// </summary>
	private Task WaitUntilPlayerFinishedInterruption()
	{
		var tcs = new TaskCompletionSource<bool>();
		EventHandler onConditionMet = null;
		onConditionMet = (sender, args) =>
		{
			if (_playerInterruption)
			{
				return;
			}
			
			tcs.TrySetResult(true);
			PlayerInterruptedRoutine -= onConditionMet;
		};
		
		PlayerInterruptedRoutine += onConditionMet;

		if (!_playerInterruption)
		{
			PlayerInterruptedRoutine?.Invoke(this, EventArgs.Empty);
		}
		
		return tcs.Task;
	}

	/// <summary>
	///		This method will interrupt the currently active Routine.
	///
	///		Important Note: Interrupting a Routine WON'T interrupt the current Task, but rather stop the next one from starting.
	/// </summary>
	public void InterruptRoutine()
	{
		_logger.Debug("Interrupting Routine.");
		_playerInterruption = true;
	}

	public void ContinueRoutine()
	{
		_logger.Debug("Resume Routine.");
		_playerInterruption = false;
		PlayerInterruptedRoutine?.Invoke(this, EventArgs.Empty);
	}
	//------------------------------------------------------------------------------------------------------------------------------------//
}
