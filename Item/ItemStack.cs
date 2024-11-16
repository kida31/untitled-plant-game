
namespace untitledplantgame.Item;

/**
 * Despite what the name suggests, every single item is an item stack. None stackable items are just that, a stack with certain type of
 * item that restricts the item stack, so it can only hold a single copy.
 */
public class ItemStack
{
	private readonly IItemType _itemType;
	private int _quantity;

	public ItemStack(IItemType itemType)
	{
		_itemType = itemType;
		_quantity = _itemType.GetItemStackSize();
	}

	public IItemType GetItemType()
	{
		return _itemType;
	}

	public int Amount
	{
		get => _quantity;
		set => _quantity = value;
	}

	public string Name => _itemType.GetItemName();

	// Add, Remove, ...
}
