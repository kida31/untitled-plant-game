using System;
using Godot;

// TODO: Each NPC should not need to be its own class
// TODO: Refactor to use a single NPC class with a property for the NPC's name and other common properties
public partial class NPCMother : AInteractable
{
	public override void Interact()
	{
		//Just so that the test scene still works. In future NPCs need to be created and allocated their dialog
		GD.Print("Mother says: Hello Kid!");
	}
}
