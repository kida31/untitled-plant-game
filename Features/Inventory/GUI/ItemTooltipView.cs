namespace untitledplantgame.Inventory.GUI;

public partial class ItemTooltipView : TooltipView
{
	public IItemStack ItemStack
	{
		get => _itemStack;
		set => SetItemStack(value);
	}

	private IItemStack _itemStack;

	private void SetItemStack(IItemStack value)
	{
		_itemStack = value;
		Title = _itemStack?.Name ?? "";
		Description = _itemStack?.Description ?? "";
	}
}
