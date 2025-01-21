using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.NPC.NpcTask;
using untitledplantgame.NPC.NpcType;
using untitledplantgame.NPC.RoutinePlanner;

namespace untitledplantgame.NPC.Routine;

// TODO: Cleanup
public partial class NpcRoutine : Node
{
	public enum Options { TimeOfDay, PlayerInteraction }
	[Export] public Options RoutineTrigger { get; set; } = Options.TimeOfDay;
	
	[Export(PropertyHint.Range, "0,23")]
	public byte RoutineStartHours { get; set; } = 7;

	[Export(PropertyHint.Range, "0,59")]
	public byte RoutineStartMinutes { get; set; } = 30;

	private bool _correctTimeOfDay;
	private bool _playerInteracted;
	private NpcRoutinePlanner _owningRoutinePlanner;
	private List<INpcTask> _npcTasks;
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
	}

	private async void TimeToTriggerRoutine()
	{
		await Task.Yield();
		
		await Task.Delay(1);
		_correctTimeOfDay = true;
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
