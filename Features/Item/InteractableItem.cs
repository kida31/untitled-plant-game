using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.NPC;

namespace untitledplantgame.Item;

public partial class InteractableItem : AInteractable
{
	public override string GetActionName() => "GAME_ACTION_PICK_UP2";

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
