using Godot;
using untitledplantgame.Common;

public partial class NPC2 : AbstractNPC, IInteractable
{
	public override void _Ready()
	{
		AddToGroup(Group.Interactables);
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
