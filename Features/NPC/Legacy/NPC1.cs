using System.Threading;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.NPC;

// TODO: Each NPC should not need to be its own class
// TODO: Refactor to use a single NPC class with a property for the NPC's name and other common properties
public partial class NPC1 : AInteractable
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
