using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;

public partial class InventoryItemView : Clickable, ITooltipable
{
	// public Action Pressed;

	[Export] private TextureRect _itemTextureRect;
	[Export] private Label _displayItemName;
	[Export] private Label _itemCurrentQuantity;
	// [Export] private Button _inventoryItemViewButton;

	private Logger _logger;
	
	public int Id;
	public IItemStack ItemStack;

	public string Title => ItemStack?.Name;

	public string Description => ItemStack?.ToolTipDescription;

	public Control Content => null;

	public override void _Ready()
	{
		_logger = new Logger(this);
		
		FocusEntered += () =>
		{
			_logger.Debug($"[{Name}] Entered");
		};
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
			_displayItemName.Text = itemStack.Name ?? "Placeholder_ItemName";
			_itemCurrentQuantity.Text = itemStack.Amount.ToString();
			_itemTextureRect.Texture = itemStack.Icon;

			// TooltipText = itemStack.Name ?? "Placeholder_Tooltip";

			ItemStack = itemStack;
		}
	}
}
