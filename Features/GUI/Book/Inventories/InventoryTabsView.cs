using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Inventory.PlayerInventory.Views;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public partial class InventoryTabsView : Control
{
	[Export] private PackedScene _inventoryViewPrefab;
	[Export] private TabContainer _tabContainer;

	// public TabContainer TabContainer => GetNode<TabContainer>("."); // Either this or make this class extend TabContainer

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
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (!Visible)
		{
			return;
		}
		
		if (@event.IsActionPressed(Book.BumperLeft))
		{
			PreviousTab(false);
		}
		else if (@event.IsActionPressed(Book.BumperRight))
		{
			NextTab(false);
		}
	}

	private void PreviousTab(bool wrapAround = true)
	{
		var tabContainer = _tabContainer;
		if (wrapAround)
		{
			var nextOrFirstTabIndex = (tabContainer.CurrentTab + 1) % tabContainer.GetChildCount();
			tabContainer.CurrentTab = nextOrFirstTabIndex;
		}
		else
		{
			tabContainer.CurrentTab = Math.Max(0, tabContainer.CurrentTab - 1);
		}

	}
	
	private void NextTab(bool wrapAround = true)
	{
		var tabContainer = _tabContainer;
		var count = tabContainer.GetChildCount();
		if (wrapAround)
		{
			tabContainer.CurrentTab = (tabContainer.CurrentTab + count - 1) % count;
		}
		else
		{
			tabContainer.CurrentTab = Mathf.Min(tabContainer.CurrentTab + 1, count - 1);
		}
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

	private void SetPotentialItemSlot(InventoryItemView inventoryItemView)
	{
		_potentialItemSlot = inventoryItemView;
	}

	private InventoryItemView GetPotentialItemSlot()
	{
		return _potentialItemSlot;
	}
}
