using Godot;
using untitledplantgame.Common;
using untitledplantgame.NPC;

// TODO: Each NPC should not need to be its own class
// TODO: Refactor to use a single NPC class with a property for the NPC's name and other common properties
public partial class NPC2 : AInteractable
{
	public override void _Ready()
	{
		AddToGroup(GameGroup.Interactables);
		var eventBus = GetNode<EventBus>("/root/EventBus");
		eventBus.Connect("NPCInteracted", new Callable(this, nameof(OnNPCInteracted)));

		//Rework with C# Events
		eventBus.NPCInteracted += OnNPCInteracted; //Yay :D
	}

	private void OnNPCInteracted(Node npc)
	{
		// Only react if the interaction is from NPC1
		if (npc is NPC1)
		{
			GD.Print("NPC2 COOOOOOOOOOOOOOOOOOOODEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
			MoveLocalX(10);
		}
	}

	public override void Interact() { }
}
