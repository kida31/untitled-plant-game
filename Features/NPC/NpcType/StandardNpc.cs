using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.NPC.NpcInteraction;
using untitledplantgame.NPC.RoutinePlanner;

namespace untitledplantgame.NPC.NpcType;

/**
 * A classic implementation of an NPC. Has a name, a portrait and a bunch of routines and dialogue options.
 */
public partial class StandardNpc : Npc
{
	[Export] private string _name;
	[Export] private NpcPlayerInteraction _npcPlayerInteraction;
	[Export] private NpcRoutinePlanner _routinePlanner;

	private Npc _npcExecutingTheseTasks;
	private Logger _logger;

	public override void _Ready()
	{
		base._Ready();
		_logger = new Logger(this);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public override string GetNpcName()
	{
		//_logger.Info("Npc name was requested.");
		return _name;
	}

	public void AssignMethodToInteractionEvent(Action action)
	{
		_npcPlayerInteraction.InteractionEvent += action;
	}
}
