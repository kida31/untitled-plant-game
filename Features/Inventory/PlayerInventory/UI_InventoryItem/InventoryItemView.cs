using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;

public partial class InventoryItemView : Control
{
	[Export] private Texture2D _specificItemIcon;
	[Export] private Label _displayItemName;
	[Export] private Label _itemCurrentQuantity;
	[Export] private TextureRect _itemTextureRect;

	public int Id;
	private Logger _logger;
	public ItemStack ItemStack;
	
	public override void _Ready()
	{
		_logger = new Logger(this);
		
		Connect(SignalName.MouseEntered, Callable.From(OnMouseEntered)); 
		Connect(SignalName.MouseExited, Callable.From(OnMouseExited));
		
		FocusEntered += () =>
		{
			_itemTextureRect.Hide();
			_logger.Debug($"[{Name}] Entered");
		};

		FocusExited += () =>
		{
			_itemTextureRect.Show();
			_logger.Debug($"[{Name}] Exited");
		};
	}

	public void UpdateItemView(ItemStack itemStack)
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
			_itemCurrentQuantity.Text = itemStack.Amount.ToString();
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
	}

	private void OnMouseExited()
	{
		EventBus.Instance.SetItemSlot(null);
	}
}
