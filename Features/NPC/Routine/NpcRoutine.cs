using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.NPC.NpcTask;
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

	private bool _correctTimeOfDay;
	private NpcRoutinePlanner _owningRoutinePlanner;
	private List<INpcTask> _npcTasks;
	private event EventHandler RightTimeOfDayReached;

	public override void _Ready()
	{
		if (RoutineTrigger == Options.TimeOfDay)
		{
			TimeController.Instance.AddEvent(
				RoutineStartHours, 
				RoutineStartMinutes, 
				TimeToTriggerRoutine
				);
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
		await Task.Yield();
		await WaitUntilCorrectTimeOfDay();
		
		foreach (var npcTask in _npcTasks)
		{
			_owningRoutinePlanner.ActiveTask = npcTask;
			npcTask.InitializeTask(_owningRoutinePlanner.GetNpcExecutingRoutines());
			await npcTask.ExecuteNpcTask();
		}

		_owningRoutinePlanner.ActiveTask = null;
	}

	private void TimeToTriggerRoutine()
	{
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
}
