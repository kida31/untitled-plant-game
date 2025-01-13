using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.NPC.Type;

public partial class StandardNpc : Npc
{
	[Export] private string _name;
	[Export] private Texture2D _defaultPortrait;
	//[Export] private Array<Routine> _routines;
	
	public override string ActionName => "pickup";

	public override void _Ready()
	{
		base._Ready(); // Yeah, I mean, that makes sense... I override the AInteractable... but I need it, lol
		GD.Print("Testing");
	}

	public override void Interact()
	{
		GD.Print("I just wanna check");
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
