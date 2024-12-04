using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Item;

public partial class InteractableItem : AInteractable
{
	public override string ActionName => "pickup";

	public ItemStack ItemStack { get; private set; }

	public InteractableItem() : this(null)
	{
	}

	public InteractableItem(ItemStack item)
	{
		ItemStack = item;
	}

	public override void Interact()
	{
		EventBus.Instance.ItemPickedUp(ItemStack);
		QueueFree();
	}
}
