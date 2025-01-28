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

	[Export] private NpcRoutine _nextRoutine;
	
	private bool _correctTimeOfDay;
	private bool _playerInteracted;
	private bool _previousRoutineFinished;
	private bool _isStartingRoutine;
	
	private NpcRoutinePlanner _owningRoutinePlanner;
	private List<INpcTask> _npcTasks;
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
		
		_nextRoutine?.PreviousRoutineFinishTask(); // The next Routine can be null. Godot won't catch the exception ⇒ NPC is stuck
	}

	public void MakeThisRoutineTheStartingPoint()
	{
		_isStartingRoutine = true;
	}

	private void PreviousRoutineFinishTask()
	{
		GD.Print("3: PreviousRoutine finished.");
		_previousRoutineFinished = true;
		PreviousRoutineFinishedTask?.Invoke(this, EventArgs.Empty);
	}

	private Task WaitUntilPreviousRoutineFinished()
	{
		var tcs = new TaskCompletionSource<bool>();
		GD.Print("2: I expect _previousRoutine to be true, I get: " + _previousRoutineFinished);
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

	private async void TimeToTriggerRoutine()
	{
		await Task.Yield();
		await Task.Delay(1);

		if (!_isStartingRoutine)
		{
			GD.Print("1: We wait until the previous Routine finishes");
			await WaitUntilPreviousRoutineFinished();
			GD.Print("4. Wait for previous routine is over!");
			
			// Have fun doing this for a player interaction right here xD
			// God damn it... I Really, really, really, hate this.
			// Jokes aside, I just need to get the current active routine and set the variable here to something blocking it.
			// Exactly like with the waiting for task finished, but this time coupled to the dialogue being finished.
		}
		
		_correctTimeOfDay = true;
		_previousRoutineFinished = false;
		//PreviousRoutineFinishTask(); // if this is null, nothing happens
		GD.Print("5: Finally, routine gets started!");
		RightTimeOfDayReached?.Invoke(this, EventArgs.Empty);
	}

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

	private void TriggerPlayerRoutineAfterInteraction()
	{
		_playerInteracted = true;
		PlayerInteracted?.Invoke(this, EventArgs.Empty);
	}
	
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
}
