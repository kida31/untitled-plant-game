namespace untitledplantgame.Inventory.GUI;

public partial class ItemTooltipView : TooltipView
{
	public ItemStack ItemStack
	{
		get => _itemStack;
		set => SetItemStack(value);
	}

	private ItemStack _itemStack;

	private void SetItemStack(ItemStack value)
	{
		_itemStack = value;
		Title = _itemStack?.Name ?? "";
		Description = _itemStack?.Description ?? "";
	}
}
