using Godot;
using untitledplantgame.Event;

public partial class NPC1 : Area2D, IInteractable
{
	[Export]
	private string _npcName;

	[Export]
	private NpcLogic _npcLogicNode;

	public override void _Ready()
	{
		AddToGroup("Interactables");
	}

	private void OnBodyEntered(Node body)
	{
		_npcLogicNode.ManageNpcCollisionWithPlayer(body, _npcName);
	}

	public Vector2 GetGlobalInteractablePosition()
	{
		return GlobalPosition;
	}

	public void Interact()
	{
		GD.Print("Talking to NPC1");

		// Broadcast the interaction globally
		EventBus eventBus = GetNode<EventBus>("/root/EventBus");
		eventBus.NotifyNPCInteracted(this);

		//_npcLogicNode.InteractionLogic();
	}
}
