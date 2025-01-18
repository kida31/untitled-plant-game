using untitledplantgame.Common;
using untitledplantgame.GUI.Book.Inventories;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Items;

/// <summary>
///     Wraps InventoryItemView while adding default functionality for pressing on slots
///     This is intended to be used for any storage screen (inventory, chest, etc.).
///     By assigning an inventory handler and a slot index, this view will handle the click and secondary click events automatically.
/// </summary>
public partial class StorageItemView : InventoryItemView
{
	public new IItemStack ItemStack => _slotIndex < 0 ? null : _inventory?.GetItem(SlotIndex);

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

	private int _slotIndex = -1;
	private IInventory _inventory;
	private Logger _logger;

	public override void _Ready()
	{
		base._Ready();
		_logger = new Logger(this);
		Pressed += OnPressed;
		SecondaryPressed += OnSecondaryPressed;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		Pressed -= OnPressed;
		SecondaryPressed -= OnSecondaryPressed;
	}

	private void UpdateContent()
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

		UpdateItemView(Inventory.GetItem(SlotIndex));
	}

	private void OnPressed()
	{
		if (_slotIndex < 0)
		{
			_logger.Error("Press: SlotIndex not set");
			return;
		}

		if (_inventory == null)
		{
			_logger.Error("Press: Inventory not set");
			return;
		}

		if (!CursorInventory.Instance.CanClick(_inventory, _slotIndex))
		{
			// This seems inconsistent with secondary click
			return;
		}

		_logger.Debug($"Handle click on {_inventory.Name}[{_slotIndex}]");
		CursorInventory.Instance.HandleClick(_inventory, _slotIndex);
	}

	private void OnSecondaryPressed()
	{
		if (_slotIndex < 0)
		{
			_logger.Error("SecondaryPress: SlotIndex not set");
			return;
		}

		if (_inventory == null)
		{
			_logger.Error("SecondaryPress: Inventory not set");
			return;
		}

		_logger.Debug($"Handle secondary click on {_inventory.Name}[{_slotIndex}]");
		CursorInventory.Instance.HandleSecondary(_inventory, _slotIndex);
	}
}
