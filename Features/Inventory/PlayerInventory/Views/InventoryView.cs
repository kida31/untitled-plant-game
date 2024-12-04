using System.Collections.Generic;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

namespace untitledplantgame.Inventory.PlayerInventory.Views;

// View of a single inventory
public partial class InventoryView : Control
{
	// [Export] public string HerbsTabName;
	// [Export] public string HerbsTabDescription;
	[Export] private PackedScene _inventoryItemScene;
	[Export] private PackedScene _tabItemView;
	[Export] private Control _inventoryItemViewContainer;

	private TabItemView _herbsItemView;
	private ItemStack[] _itemStacks;
	private List<InventoryItemView> _inventoryItemViews;

	public void UpdateInventory(IInventory inventory)
	{
		var items = inventory.GetItems();
		FillTabWithEmptyInventoryItemViews(items.Count);
		// Populate content of the item views
		for (int i = 0; i < _inventoryItemViews.Count; i++)
		{
			_inventoryItemViews[i].UpdateItemView(i < items.Count ? items[i] : null);
		}
	}

	private void FillTabWithEmptyInventoryItemViews(int newCount)
	{
		Assert.AssertTrue(_inventoryItemViews.Count == _inventoryItemViewContainer.GetChildCount(),
			"Tracked nodes and children of container should be same");
		while (_inventoryItemViews.Count > newCount)
		{
			var itemView = _inventoryItemViews[0];
			_inventoryItemViews.Remove(itemView);
			itemView.QueueFree();
		}

		while (_inventoryItemViews.Count < newCount)
		{
			var itemView = _inventoryItemScene.Instantiate<InventoryItemView>();
			_inventoryItemViewContainer.AddChild(itemView);
			_inventoryItemViews.Add(itemView);
		}
	}
}
