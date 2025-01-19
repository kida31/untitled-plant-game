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
	private Logger _logger;
	
	public override void _Ready()
	{
		ExecuteAllRoutines(); 
		_logger = new Logger(this);
	}

	public Npc GetNpcExecutingRoutines()
	{
		return _npcExecutingRoutines;
	}
	
	private async void ExecuteAllRoutines()
	{
		await Task.Delay(16); // Script execution order delay needed
		_logger.Debug("Startin");
		EventBus.Instance.NpcDialogueWithPlayerStarted(
			(AnimatedSprite2D) _npcExecutingRoutines.GetNode("PortraitSprite2D"),
			_npcExecutingRoutines.GetNpcName()
			);
		
		foreach (var npcRoutine in _routines)
		{
			npcRoutine.InitializeRoutine(this);
			await npcRoutine.ExecuteAllTasks();
		}
	}
}
