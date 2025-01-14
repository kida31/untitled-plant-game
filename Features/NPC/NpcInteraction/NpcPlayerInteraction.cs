using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.NPC.NpcInteraction;

public partial class NpcPlayerInteraction : AInteractable
{
	public override string ActionName => "talk to";
	
	public override void Interact()
	{
		GD.Print("Hello Player!");
	}

	public override void _Process(double delta)
	{
		
	}
}
