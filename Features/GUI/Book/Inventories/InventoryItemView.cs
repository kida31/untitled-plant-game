using Godot;
using untitledplantgame.Common;
using untitledplantgame.GUI.Components;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Book.Inventories;

/// <summary>
///     This class is a view for an inventory item. It displays the name, quantity, and icon.
///     Note: The name is currently hidden per default.
/// 	TODO: This will be renamed. Consider using NewInventoryItemView
///		The difference between this and InventoryItemView is that this one sets the item directly.
///		While the other uses IInventory handler and index as connection to the inventory system.
/// </summary>
public partial class InventoryItemView : Clickable, ITooltipable
{
	[Export] private Label _displayItemName;
	[Export] private Label _itemCurrentQuantity;
	[Export] private TextureRect _itemTextureRect;

	private Logger _logger;
	public IItemStack ItemStack { get; private set; } // Use UpdateItemView to set it
	public string Title => ItemStack?.Name;
	public string Description => ItemStack?.ToolTipDescription;
	public Control Content => null;

	public override void _Ready()
	{
		_logger = new Logger(this);

		FocusEntered += () => { _logger.Debug($"[{Name}] Entered"); };
	}

	public void UpdateItemView(IItemStack itemStack)
	{
		if (itemStack == null)
		{
			_displayItemName.Text = "";
			_itemCurrentQuantity.Text = "";
			_itemTextureRect.Texture = null;

			ItemStack = null;
		}
		else
		{
			// Placeholders: 
			_displayItemName.Text = itemStack.Name;
			_itemCurrentQuantity.Text = itemStack.Amount.ToString();
			_itemTextureRect.Texture = itemStack.Icon;

			// TooltipText = itemStack.Name ?? "Placeholder_Tooltip";

			ItemStack = itemStack;
		}
	}
}
