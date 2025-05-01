using Godot;
using untitledplantgame.NPC;

public partial class Door : AInteractable
{
	[Export]
	private string _actionName = "GAME_ACTION_INTERACT";

	public override string GetActionName() => _actionName;

	[Export]
	public string entryDoorName { get; private set; }

	public override void Interact()
	{
		TeleportPlayer.Instance.TPP(this);
	}
}
