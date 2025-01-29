using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.NPC.NpcTask;
using untitledplantgame.NPC.NpcType;
using untitledplantgame.NPC.RoutinePlanner;

namespace untitledplantgame.NPC.Routine;


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
			//GD.Print("1. I start the routine once, but I expect this to happen 3 time!");
			await WaitUntilCorrectTimeOfDay();
		}
		else 
		{
			await WaitUntilPlayerInteracted();
		}
		
		//GD.Print("9. Letting us execute the task");
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
		//GD.Print("3. When the time is right, the actual Routine begins");
		await Task.Yield();
		await Task.Delay(1);
		
		if (!_isStartingRoutine)
		{
			await WaitUntilPreviousRoutineFinished();
			_owningRoutinePlanner.LastRoutine = this;
			//GD.Print("4. I am waiting for the player to finish the Dialogue!");
			await WaitUntilPlayerFinishedInterruption(); // No need to account for interruption WITHIN tasks, is handled elsewhere
		}
		else
		{
			_owningRoutinePlanner.LastRoutine = this;
			//GD.Print("4. I am waiting for the player to finish the Dialogue!");
			await WaitUntilPlayerFinishedInterruption(); // No need to account for interruption WITHIN tasks, is handled elsewhere
		}

		_correctTimeOfDay = true;
		_previousRoutineFinished = false;
		
		RightTimeOfDayReached?.Invoke(this, EventArgs.Empty);
	}
	//------------------------------------------------------------------------------------------------------------------------------------//
	
	
	/// <summary>
	///		Scenario 1:
	///		Handles the scenario when a the routine is based on a specific time of day
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
			//GD.Print("8. We can give the go to the correct timing.");
			tcs.TrySetResult(true);
			RightTimeOfDayReached -= onConditionMet;
		};
		//GD.Print("2. The Task is executed and waits on it's event.");
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
			//GD.Print("7. And when the task is fulfilled!...");
			tcs.TrySetResult(true);
			PlayerInterruptedRoutine -= onConditionMet;
		};
		
		PlayerInterruptedRoutine += onConditionMet;

		if (!_playerInterruption)
		{
			PlayerInterruptedRoutine?.Invoke(this, EventArgs.Empty);
		}
		
		//GD.Print("5. I wait for the dialogue, cause the player intervened.");
		return tcs.Task;
	}

	public void InterruptRoutine()
	{
		_playerInterruption = true;
	}

	public void ContinueRoutine()
	{
		//GD.Print("6. The player resumes the Routine when he is finished.");
		_playerInterruption = false;
		PlayerInterruptedRoutine?.Invoke(this, EventArgs.Empty);
	}
	//------------------------------------------------------------------------------------------------------------------------------------//
}
