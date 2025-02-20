using System;

namespace untitledplantgame.NPC.NpcInteraction;

/// <summary>
///     Works exactly like the normal (Player) Interaction, but this one is specifically designed for Npcs only.
///		If a Npc is supposed to have an Interaction with the player, a method can be subscribed to the internal, public Action.
/// </summary>
public partial class NpcPlayerInteraction : AInteractable
{
	public Action InteractionEvent;
	public override string ActionName => "Talk";
	
	/// <summary>
	///		Invokes the NpcPlayerInteraction Event. If no method was subscribed to the Action and this method is called, nothing happens.
	/// </summary>
	public override void Interact()
	{
		InteractionEvent?.Invoke();
	}
}
