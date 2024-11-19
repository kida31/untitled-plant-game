using System;
using Godot;

public partial class NPCMother : AbstractNPC
{
	public override void Interact()
	{
		//Just so that the test scene still works. In future NPCs need to be created and allocated their dialog
		GD.Print("Mother says: Hello Kid!");
	}
}
