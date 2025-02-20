using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.NPC.NpcInteraction;
using untitledplantgame.NPC.RoutinePlanner;

namespace untitledplantgame.NPC.NpcType;

/// <summary>
///		A classic implementation of an NPC. Has a name, a portrait and a bunch of routines and dialogue options.
/// </summary>
public partial class StandardNpc : Npc
{
	[Export] private string _name;
	[Export] private bool _disableMovement;
	[Export] private NpcPlayerInteraction _npcPlayerInteraction;
	[Export] private NpcRoutinePlanner _routinePlanner;
	[Export] private AnimatedSprite2D _overWorldSprite;

	private Npc _npcExecutingTheseTasks;
	private Logger _logger;

	public override void _Ready()
	{
		base._Ready();
		_overWorldSprite?.Play();
		_logger = new Logger(this);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (!_disableMovement)
		{
			MoveAndSlide();
		}
	}

	public override string GetNpcName()
	{
		_logger.Info("Npc name was requested.");
		return _name;
	}

	/// <summary>
	///		Assigns a method to the Player Interaction Event.
	/// </summary>
	/// <param name="action"></param>
	public void AssignMethodToInteractionEvent(Action action)
	{
		_npcPlayerInteraction.InteractionEvent += action;
	}
}
