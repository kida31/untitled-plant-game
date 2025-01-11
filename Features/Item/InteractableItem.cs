using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Item;

public partial class InteractableItem : AInteractable
{
	public override string ActionName => "pickup";

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
