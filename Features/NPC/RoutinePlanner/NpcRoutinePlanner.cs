using System;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.NPC.NpcTask;
using untitledplantgame.NPC.Routine;

namespace untitledplantgame.NPC.RoutinePlanner;

public partial class NpcRoutinePlanner : Node
{
	[Export] private Npc _npcExecutingRoutines;
	[Export] private Array<NpcRoutine> _routines;
	
	public INpcTask ActiveTask;
	
	public override void _Ready()
	{
		ExecuteAllRoutines(); 
	}

	public Npc GetNpcExecutingRoutines()
	{
		return _npcExecutingRoutines;
	}

	public void SetActiveTask(INpcTask task)
	{
		ActiveTask = task;
	}
	
	private async void ExecuteAllRoutines()
	{
		await Task.Delay(10); // Script execution order
		
		EventBus.Instance.NpcDialogueWithPlayerStarted((AnimatedSprite2D) _npcExecutingRoutines.GetNode("PortraitSprite2D"));
		
		foreach (var npcRoutine in _routines)
		{
			npcRoutine.InitializeRoutine(this);
			await npcRoutine.ExecuteAllTasks();
		}
	}
}
