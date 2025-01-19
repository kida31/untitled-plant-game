using untitledplantgame.GUI;

namespace untitledplantgame.Inventory.GUI;

/// <summary>
///     Tooltip view for items.
/// </summary>
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
		Description = _itemStack?.ToolTipDescription ?? "";
	}
}
