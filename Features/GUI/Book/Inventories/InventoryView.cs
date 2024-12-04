using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

namespace untitledplantgame.Inventory.PlayerInventory.Views;

// View of a single inventory
public partial class InventoryView : Control
{
	[Export] private PackedScene _inventoryItemViewPrefab;
	[Export] private Container _inventoryItemViewContainer;

	private List<InventoryItemView> _inventoryItemViews;

	public void UpdateInventory(IInventory inventory)
	{
		// Consider caching children and values instead of querying them every time
		_inventoryItemViews = _inventoryItemViewContainer.GetChildren().OfType<InventoryItemView>().ToList();
		
		var items = inventory.GetItems();
		FillTabWithEmptyInventoryItemViews(items.Count);
		
		// Populate content of the item views
		for (int i = 0; i < _inventoryItemViews.Count; i++)
		{
			var view = _inventoryItemViews[i];
			view.UpdateItemView(i < items.Count ? items[i] : null);
		}
	}

	/// <summary>
	/// Removes or adds item views to match the new item count
	/// </summary>
	/// <param name="newCount"></param>
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
			var itemView = _inventoryItemViewPrefab.Instantiate<InventoryItemView>();
			_inventoryItemViewContainer.AddChild(itemView);
			_inventoryItemViews.Add(itemView);
		}
	}
}
