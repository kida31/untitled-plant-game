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

	public string Description => ItemStack?.Description;

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

	// public override Variant _GetDragData(Vector2 atPosition)
	// {
	// 	// This is your custom method generating the preview of the drag data. Can be any Control node.
	// 	var textureRect = new TextureRect();
	// 	textureRect.Texture = _itemTextureRect.Texture;
	// 	textureRect.PivotOffset = textureRect.Size / 2;
	// 	SetDragPreview(textureRect);
	// 	
	// 	_itemTextureRect.Texture = null; // simulate drag; 
	// 	return this;
	// }

	// public override bool _CanDropData(Vector2 atPosition, Variant data)
	// {
	// 	return true;
	// }
	//
	//
	// public override void _DropData(Vector2 atPosition, Variant data)
	
	// {
	// 	var previousSlot = (InventoryItemView) data;
	// 	EventBus.Instance.InventoryItemMoved(previousSlot.ItemStack, EventBus.Instance.GetItemSlot()); // Remember, this has to be searched for!
	// 	
	// 	previousSlot.ClearItemViewData();
	// }

	// private void ClearItemViewData()
	// {
	// 	UpdateItemView(null);
	// }
}
