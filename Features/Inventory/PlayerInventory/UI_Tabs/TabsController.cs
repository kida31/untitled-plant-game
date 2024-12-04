using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Inventory.PlayerInventory.Views;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public partial class TabsController : Control
{
	[Export] private PackedScene _inventoryViewPrefab;
	private InventoryItemView _potentialItemSlot;

	private List<InventoryView> _tabs;


	public override void _Ready()
	{
		// EventBus.Instance.OnTabsUpdate += AddItemToCorrespondingTab;
		// EventBus.Instance.OnInventoryItemMove += DropInventoryItemToNewSlot; // different

		EventBus.Instance.OnSetItemSlot += SetPotentialItemSlot;
		EventBus.Instance.OnGetItemSlot += GetPotentialItemSlot;

		_tabs ??= GetChildren().OfType<InventoryView>().ToList();
	}

	public void UpdateInventories(List<IInventory> inventories)
	{
		// Remove excess tabs
		while (_tabs.Count > inventories.Count)
		{
			var tabToRemove = _tabs.Last();
			_tabs.Remove(tabToRemove);
			RemoveChild(tabToRemove);
			tabToRemove.QueueFree();
		}

		// Add tabs based on the inventories list
		while (_tabs.Count < inventories.Count)
		{
			var newTab = _inventoryViewPrefab.Instantiate<InventoryView>();
			AddChild(newTab);
			_tabs.Add(newTab);
		}
		
		Assert.AssertEquals(inventories.Count, _tabs.Count, "Inventories and tabs count mismatch");
		for (int i = 0; i < inventories.Count; i++)
		{
			var inventory = inventories[i];
			var tab = _tabs[i];
			tab.UpdateInventory(inventory);
			tab.Name = inventory.Name;
		}
	}

	/**
	 * Checks if first-degree nodes are of the type "ICategoryTab" and returns them.
	 */
	private List<T> GetChildrenOfType<T>()
		where T : ICategoryTab
	{
		var childrenOfType = new List<T>();
		for (var i = 0; i < this.GetChildCount(); i++)
		{
			var child = this.GetChild(i);
			if (child is T typedChild)
			{
				childrenOfType.Add(typedChild);
			}
		}

		return childrenOfType;
	}

	private void SetPotentialItemSlot(InventoryItemView inventoryItemView)
	{
		_potentialItemSlot = inventoryItemView;
	}

	private InventoryItemView GetPotentialItemSlot()
	{
		return _potentialItemSlot;
	}
}
