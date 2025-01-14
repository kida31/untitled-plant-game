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

	public override void _Ready()
	{
		base._Ready();
		
		//GD.Print(GetType());
		//GD.Print("Testing");
	}

	public override string GetNpcName()
	{
		throw new System.NotImplementedException();
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
