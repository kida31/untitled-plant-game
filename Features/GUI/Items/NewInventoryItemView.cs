using Godot;
using untitledplantgame.Common;
using untitledplantgame.GUI.Components;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Items;

/// <summary>
///     Wraps ItemView while changing
///     This is intended to be used for any storage screen (inventory, chest, etc.).
///     By assigning an inventory handler and a slot index, this view will handle the click and secondary click events automatically.
/// </summary>
public partial class NewInventoryItemView : Clickable, ITooltipable
{
	// Exports (Setup)
	
	[Export] private Label _itemCurrentQuantity;
	[Export] private TextureRect _itemTextureRect;
	
	// Private fields
	
	private int _slotIndex = -1; // TODO: Consider making their properties one combined setter method instead
	private IInventory _inventory;
	private Logger _logger;

	// Read-only properties
	public virtual string Title => ItemStack?.Name;
	public virtual string Description => ItemStack?.ToolTipDescription;
	public virtual Control Content => null;
	public IItemStack ItemStack => _slotIndex < 0 ? null : _inventory?.GetItem(SlotIndex);
	
	// Properties
	public IInventory Inventory
	{
		get => _inventory;
		set
		{
			if (_inventory != null)
			{
				_inventory.InventoryChanged -= UpdateContent;
			}

			_inventory = value;
			_inventory.InventoryChanged += UpdateContent;
		}
	}
	public int SlotIndex
	{
		get => _slotIndex;
		set
		{
			_slotIndex = value;
			UpdateContent();
		}
	}

	// Methods
	
	public override void _Ready()
	{
		_logger = new Logger(this);
	}

	protected virtual void UpdateContent()
	{
		if (_slotIndex < 0)
		{
			_logger.Debug("UpdateContent: SlotIndex not set");
			return;
		}

		if (_inventory == null)
		{
			_logger.Error("UpdateContent: Inventory not set");
			return;
		}

		var itemStack = Inventory.GetItem(SlotIndex);
		
		if (itemStack == null)
		{
			_itemCurrentQuantity.Text = "";
			_itemTextureRect.Texture = null;
		}
		else
		{
			_itemCurrentQuantity.Text = itemStack.Amount.ToString();
			_itemTextureRect.Texture = itemStack.Icon;
		}
	}
}
