using Godot;
using untitledplantgame.NPC;

public partial class Door : AInteractable
{

	[Export]
	public string entryDoorName { get; private set; }

	public override void Interact()
	{
		TeleportPlayer.Instance.TPP(this);
	}
}
