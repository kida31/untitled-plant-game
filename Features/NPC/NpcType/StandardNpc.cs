using Godot;
using untitledplantgame.NPC.NpcInteraction;
using untitledplantgame.NPC.RoutinePlanner;

namespace untitledplantgame.NPC.NpcType;

/**
 * A classic implementation of an NPC. Has a name, a portrait and a bunch of routines and dialogue options.
 */
public partial class StandardNpc : Npc
{
	[Export] private string _name;
	[Export] private Texture2D _defaultPortrait;
	[Export] private NpcPlayerInteraction _npcPlayerInteraction;
	[Export] private NpcRoutinePlanner _routinePlanner;

	private Npc _npcExecutingTheseTasks;

	public override void _Ready()
	{
		base._Ready();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public override string GetNpcName()
	{
		return _name;
	}

	public override bool IsNpcInteractable()
	{
		return true;
	}

	public override Texture2D GetNpcDefaultPortrait()
	{
		return _defaultPortrait;
	}

	public override void SetNpcDefaultPortrait(Texture2D newDefaultPortrait)
	{
		_defaultPortrait = newDefaultPortrait;
	}
}
