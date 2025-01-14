using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.NPC.NpcTask;

namespace untitledplantgame.NPC.Routine;

public partial class NpcRoutine : Node
{
	public enum Options
	{
		TimeOfDay,
		PlayerInteraction
	}

	[Export] public Options RoutineTrigger { get; set; } = Options.TimeOfDay;
	
	[Export(PropertyHint.Range, "0,23")]
	public byte RoutineStartHours { get; set; } = 7;

	[Export(PropertyHint.Range, "0,59")]
	public byte RoutineStartMinutes { get; set; } = 30;
	private List<INpcTask> _npcTasks;

	public override void _Ready()
	{
		_npcTasks = new List<INpcTask>();
		
		foreach (var node in GetChildren())
		{
			if (node is INpcTask npcTask)
			{
				_npcTasks.Add(npcTask);
			}
		}
	}
	
	public async Task ExecuteAllTasks()
	{
		/*
		var timeStamp = TimeController.Instance.TimeOfDayInMinutes;
		
		var minutes = timeStamp % 60;
		var hours = (timeStamp - minutes) / 60;

		if (hours != RoutineStartHours || minutes != RoutineStartMinutes)
		{
			return;
		}
		*/
		
		foreach (var npcTask in _npcTasks)
		{
			await npcTask.ExecuteNpcTask();
		}
	}
}
