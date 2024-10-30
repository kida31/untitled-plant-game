using Godot;
using untitledplantgame.EntityStatsDataContainer;
using untitledplantgame.Item;

/**
 * NOTE:
 *
 * Using something like "public event Action PerformedAction;" would be viable, but in order to keep things simple and
 * uniform, I decided to not use "Actions". However, this can change at any point in time if the code demands to be
 * simplified even further.
 */
public partial class EventBus : Node
{
	
	//---------------------------------------------Legacy Signals---------------------------------------------
	[Signal] public delegate void NPCInteractedEventHandler(Node npc); //Replace with C# Action
	
	public void NotifyNPCInteracted(Node npc)
	{
		EmitSignal(nameof(NPCInteracted), npc);
	}
	//---------------------------------------------Legacy Signals---------------------------------------------
	
	
	
	public delegate void AddToInventoryEventHandler(InteractableItem interactableItem);

	public event AddToInventoryEventHandler OnItemPickUp;

	public void ItemPickedUp(InteractableItem interactableItem)
	{
		OnItemPickUp?.Invoke(interactableItem); 
	}
}
