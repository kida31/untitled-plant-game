using Godot;

public partial class NPC1 : AbstractNPC
{
	public override void Interact()
	{
		GD.Print("Talking to NPC1");

		// Broadcast the interaction globally
		EventBus eventBus = GetNode<EventBus>("/root/EventBus");
		eventBus.NotifyNPCInteracted(this);

		//_npcLogicNode.InteractionLogic();
	}
}
