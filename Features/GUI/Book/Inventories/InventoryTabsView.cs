using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.Inventory.GUI;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Inventory.PlayerInventory.Views;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public partial class InventoryTabsView : Control
{
	[Export] private PackedScene _inventoryViewPrefab;
	[Export] private PackedScene _inventoryCategoryTabPrefab;

	[Export] private TabContainer _tabContainer;
	[Export] private Container _tabButtonContainer;

	// public TabContainer TabContainer => GetNode<TabContainer>("."); // Either this or make this class extend TabContainer

	private InventoryItemView _potentialItemSlot;
	private List<InventoryCategoryTab> _tabButtons;
	private List<InventoryView> _tabs;

	private Dictionary<InventoryCategoryTab, Action> _buttonActions = new();

	public override void _Ready()
	{
		// EventBus.Instance.OnTabsUpdate += AddItemToCorrespondingTab;
		// EventBus.Instance.OnInventoryItemMove += DropInventoryItemToNewSlot; // different

		EventBus.Instance.OnSetItemSlot += SetPotentialItemSlot;
		EventBus.Instance.OnGetItemSlot += GetPotentialItemSlot;

		_tabs = _tabContainer.GetChildren().OfType<InventoryView>().ToList();
		_tabButtons = _tabButtonContainer.GetChildren().OfType<InventoryCategoryTab>().ToList();

		_tabContainer.TabChanged += OnTabChanged;
		_tabContainer.CurrentTab = 0;
	}

	private void OnTabChanged(long tab)
	{
		int activeIndex = (int) tab;
		if (activeIndex < 0 || activeIndex >= _tabs.Count)
		{
			// log error
			return;
		}

		for (var i = 0; i < _tabButtons.Count; i++)
		{
			// var s = i == activeIndex ? "active" : "inactive";
			// GD.Print($"{i} as {s}");
			var button = _tabButtons[i];
			button.SetIsActive(i == activeIndex);
		}
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
		// Remove excess tabs and buttons
		while (_tabs.Count > inventories.Count)
		{
			var tabToRemove = _tabs.Last();
			_tabs.Remove(tabToRemove);
			_tabContainer.RemoveChild(tabToRemove);
			tabToRemove.QueueFree();
		}

		while (_tabButtons.Count > inventories.Count)
		{
			var buttonToRemove = _tabButtons.Last();
			_tabButtons.Remove(buttonToRemove);
			_tabButtonContainer.RemoveChild(buttonToRemove);
			buttonToRemove.QueueFree();
		}

		// Add tabs based on the inventories list
		while (_tabs.Count < inventories.Count)
		{
			var newTab = _inventoryViewPrefab.Instantiate<InventoryView>();
			_tabContainer.AddChild(newTab);
			_tabs.Add(newTab);
		}

		while (_tabButtons.Count < inventories.Count)
		{
			var newButton = _inventoryCategoryTabPrefab.Instantiate<InventoryCategoryTab>();
			_tabButtonContainer.AddChild(newButton);
			_tabButtons.Add(newButton);
		}

		Assert.AssertEquals(inventories.Count, _tabs.Count, "Inventories and tabs count mismatch");
		for (int i = 0; i < inventories.Count; i++)
		{
			// Tab
			var inventory = inventories[i];
			var tab = _tabs[i];
			tab.UpdateInventory(inventory);
			tab.Name = inventory.Name;

			//Button
			var button = _tabButtons[i];
			button.Name = inventory.Name;
			button.Text = inventory.Name;
			if (_buttonActions.ContainsKey(button))
			{
				button.Pressed -= _buttonActions[button];
			}

			void ButtonAction() => OnTabButtonPressed(button);
			_buttonActions[button] = ButtonAction;
			button.Pressed += ButtonAction;
		}
	}

	private void OnTabButtonPressed(InventoryCategoryTab button)
	{
		var index = _tabButtons.IndexOf(button);
		if (index < 0)
		{
			// Log error
		}
		else
		{
			_tabContainer.CurrentTab = index;
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
