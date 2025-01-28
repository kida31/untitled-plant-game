using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.NPC;

namespace untitledplantgame.Item;

public partial class InteractableItem : AInteractable
{
	public override string ActionName => "Pickup";

	public IItemStack ItemStack { get; private set; }

	public InteractableItem() : this(null)
	{
	}

	public InteractableItem(IItemStack item)
	{
		ItemStack = item;
	}

	public override void Interact()
	{
		EventBus.Instance.ItemPickedUp(ItemStack);
		QueueFree();
	}
}
