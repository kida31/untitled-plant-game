using untitledplantgame.Common;
using untitledplantgame.GUI.Book.Inventories;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Items;

/// <summary>
///     Wraps InventoryItemView while adding default functionality for pressing on slots
///     This is intended to be used for any storage screen (inventory, chest, etc.).
///     By assigning an inventory handler and a slot index, this view will handle the click and secondary click events automatically.
/// </summary>
public partial class StorageItemView : NewInventoryItemView
{
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

	private void OnPressed()
	{
		if (SlotIndex < 0)
		{
			_logger.Error("Press: SlotIndex not set");
			return;
		}

		if (Inventory == null)
		{
			_logger.Error("Press: Inventory not set");
			return;
		}

		if (!CursorInventory.Instance.CanClick(Inventory, SlotIndex))
		{
			// This seems inconsistent with secondary click
			return;
		}

		_logger.Debug($"Handle click on {Inventory.Name}[{SlotIndex}]");
		CursorInventory.Instance.HandleClick(Inventory, SlotIndex);
	}

	private void OnSecondaryPressed()
	{
		if (SlotIndex < 0)
		{
			_logger.Error("SecondaryPress: SlotIndex not set");
			return;
		}

		if (Inventory == null)
		{
			_logger.Error("SecondaryPress: Inventory not set");
			return;
		}

		_logger.Debug($"Handle secondary click on {Inventory.Name}[{SlotIndex}]");
		CursorInventory.Instance.HandleSecondary(Inventory, SlotIndex);
	}
}
