using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.GUI.Book.Inventories;
using untitledplantgame.GUI.Items;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Vending;

/// <summary>
///     This class is a view for the inventory. It displays the items in the inventory and allows the player to interact with them.
/// </summary>
public partial class SimplerInventoryView : Control
{
	[Export] private Container _inventoryItemViewContainer;
	[Export] private PackedScene _inventoryItemViewPrefab;

	private List<StorageItemView> _inventoryItemViews;
	[Export] private Label _itemNameLabel;

	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		_inventoryItemViews = _inventoryItemViewContainer.GetChildren().OfType<StorageItemView>().ToList();
		_inventoryItemViews.ForEach(iv => { iv.FocusEntered += () => OnItemViewFocused(iv); });
		_itemNameLabel.Text = "";

		VisibilityChanged += () =>
		{
			if (!IsVisibleInTree())
			{
				OnVisibilityOff();
			}
		};
	}

	public void UpdateInventory(IInventory inventory)
	{
		_inventoryItemViews = _inventoryItemViewContainer.GetChildren().OfType<StorageItemView>().ToList();

		var items = inventory.GetItems();
		FillTabWithEmptyInventoryItemViews(items.Count);

		// Hook up item views
		for (var i = 0; i < _inventoryItemViews.Count; i++)
		{
			var itemView = _inventoryItemViews[i];

			itemView.Inventory = inventory;
			itemView.SlotIndex = i < items.Count ? i : -1;
		}

		// Update item label depending on currently focused item view. Band-aid fix for initial focus
		var owner = GetViewport().GuiGetFocusOwner();
		if (owner is InventoryItemView iv)
		{
			OnItemViewFocused(iv);
		}
	}

	/// <summary>
	///     Focuses the first item view
	/// </summary>
	public new void GrabFocus()
	{
		_inventoryItemViews.FirstOrDefault()?.GrabFocus();
	}

	/// <summary>
	///     Removes or adds item views to match the new item count
	/// </summary>
	/// <param name="newCount"></param>
	private void FillTabWithEmptyInventoryItemViews(int newCount)
	{
		Assert.AssertTrue(_inventoryItemViews.Count == _inventoryItemViewContainer.GetChildCount(),
			$"Number of items did not match. Tracked nodes are {_inventoryItemViews.Count} and container children are {_inventoryItemViewContainer.GetChildCount()}");

		while (_inventoryItemViews.Count > newCount)
		{
			var itemView = _inventoryItemViews[0];
			_inventoryItemViews.Remove(itemView);
			itemView.QueueFree();
		}

		while (_inventoryItemViews.Count < newCount)
		{
			var itemView = _inventoryItemViewPrefab.Instantiate<StorageItemView>();
			_inventoryItemViewContainer.AddChild(itemView);
			itemView.FocusEntered += () => OnItemViewFocused(itemView);
			_inventoryItemViews.Add(itemView);
		}
		Assert.AssertTrue(_inventoryItemViews.Count == _inventoryItemViewContainer.GetChildCount(),
			$"Number of items did not match after adjustment. Tracked nodes are {_inventoryItemViews.Count} and container children are {_inventoryItemViewContainer.GetChildCount()}");
	}

	private void OnItemViewFocused(InventoryItemView itemView)
	{
		_itemNameLabel.Text = itemView.ItemStack?.Name ?? "";
	}

	private void OnVisibilityOff()
	{
		CursorInventory.Instance.ReturnPickUp();
	}
}
