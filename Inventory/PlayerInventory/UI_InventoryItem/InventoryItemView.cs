using System;
using Godot;
using untitledplantgame.Event;
using untitledplantgame.Item;

namespace untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;

public partial class InventoryItemView : Control
{
	[Export] private PackedScene _inventoryItemScene;
	[Export] private Texture2D _specificItemIcon;
	[Export] private Label _displayItemName;
	[Export] private Label _itemCurrentQuantity;
	[Export] private BaseButton _itemDeleteButton;
	[Export] private TextureRect _itemTextureRect;

	[Export] private BaseButton _descriptionButton;

	public int Id;
	
	public untitledplantgame.Item.ItemStack ItemStack;
	
	public event Action DeletePressed;
	
	public override void _Ready()
	{
		Connect(SignalName.MouseEntered, Callable.From(OnMouseEntered)); // Not sure if right
		Connect(SignalName.MouseExited, Callable.From(OnMouseExited));
		
		_itemDeleteButton.Pressed += () => DeletePressed?.Invoke();

		_descriptionButton.Pressed += SetDetailedView;
	}

	public override void _Process(double delta)
	{
		if (ItemStack != null)
		{
			//GD.Print(ItemStack);	
		}
	}

	public void UpdateItemView(untitledplantgame.Item.ItemStack itemStack)
	{
		if (itemStack == null)
		{
			_displayItemName.Text = "";
			_itemCurrentQuantity.Text = "";
			_itemDeleteButton.Visible = false;
			_itemTextureRect.Texture = null;

			ItemStack = null;
		}
		else
		{
			// Placeholders: 
			_itemCurrentQuantity.Text = "0";
			_displayItemName.Text = itemStack.Name ?? "Placeholder_ItemName";
			_itemTextureRect.Texture = _specificItemIcon;
			TooltipText = itemStack.Name ?? "Placeholder_Tooltip";

			ItemStack = itemStack;
		}
	}
	
	public override Variant _GetDragData(Vector2 atPosition)
	{
		// This is your custom method generating the preview of the drag data. Can be any Control node.
		var textureRect = new TextureRect();
		textureRect.Texture = _itemTextureRect.Texture;
		SetDragPreview(textureRect);
		
		_itemTextureRect.Texture = null; // simulate drag; 
		return this;
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return true;
	}
	
	
	public override void _DropData(Vector2 atPosition, Variant data)
	{
		var previousSlot = (InventoryItemView) data;
		EventBus.Instance.InventoryItemMoved(previousSlot.ItemStack, EventBus.Instance.GetItemSlot()); // Remember, this has to be searched for!
		
		previousSlot.ClearItemViewData();
	}
	
	private void ClearItemViewData()
	{
		UpdateItemView(null);
	}
	
	private void OnMouseEntered()
	{
		EventBus.Instance.SetItemSlot(this);
		//GD.Print($"Hovering over: {EventBus.Instance.GetItemSlot()}");
	}

	private void OnMouseExited()
	{
		EventBus.Instance.SetItemSlot(null);
		//GD.Print($"Hovering over: {EventBus.Instance.GetItemSlot()}");
	}

	private void SetDetailedView()
	{
		EventBus.Instance.UiItemClicked(_specificItemIcon, _displayItemName.Text);
	}
}
