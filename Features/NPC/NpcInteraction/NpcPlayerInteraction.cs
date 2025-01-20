using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.NPC.NpcInteraction;

public partial class NpcPlayerInteraction : AInteractable
{
	public Action InteractionEvent;
	public override string ActionName => "talk to";
	
	public override void Interact()
	{
		InteractionEvent?.Invoke();
	}
}