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
	public event Action<long> TabChanged;

	// [Export] private PackedScene _inventoryViewPrefab;
	[Export] private PackedScene _inventoryCategoryTabPrefab;

	// [Export] private TabContainer _tabContainer;
	[Export] private Container _tabButtonContainer;
	[Export] private InventoryView _inventoryView;

	private int CurrentTab
	{
		get => _currentTab;
		set
		{
			_currentTab = Math.Clamp(value, 0, _inventories.Count);
			TabChanged?.Invoke(_currentTab);
		}
	}

	private int _currentTab = -1;

	// public TabContainer TabContainer => GetNode<TabContainer>("."); // Either this or make this class extend TabContainer

	private InventoryItemView _potentialItemSlot;

	private List<InventoryCategoryTab> _tabButtons;
	// private List<InventoryView> _tabs;

	private Dictionary<InventoryCategoryTab, Action> _buttonActions = new();

	private List<IInventory> _inventories = new();

	public override void _Ready()
	{
		// EventBus.Instance.OnTabsUpdate += AddItemToCorrespondingTab;
		// EventBus.Instance.OnInventoryItemMove += DropInventoryItemToNewSlot; // different

		EventBus.Instance.OnSetItemSlot += SetPotentialItemSlot;
		EventBus.Instance.OnGetItemSlot += GetPotentialItemSlot;

		// _tabs = _tabContainer.GetChildren().OfType<InventoryView>().ToList();
		_tabButtons = _tabButtonContainer.GetChildren().OfType<InventoryCategoryTab>().ToList();

		// _tabContainer.CurrentTab = 0;
		TabChanged += OnTabChanged;
		TabChanged += (_) => CursorInventory.Instance.ReturnPickUp();
	}

	private void OnTabChanged(long tab)
	{
		int activeIndex = (int) tab;
		if (activeIndex < 0 || activeIndex >= _inventories.Count)
		{
			return;
		}

		for (var i = 0; i < _tabButtons.Count; i++)
		{
			var button = _tabButtons[i];
			button.SetIsActive(i == activeIndex);
		}

		ForceUpdateView();
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
		if (wrapAround)
		{
			var nextOrFirstTabIndex = (CurrentTab + 1) % _inventories.Count;
			CurrentTab = nextOrFirstTabIndex;
		}
		else
		{
			CurrentTab = Math.Max(0, CurrentTab - 1);
		}
	}

	private void NextTab(bool wrapAround = true)
	{
		var count = _inventories.Count;
		if (wrapAround)
		{
			CurrentTab = (CurrentTab + count - 1) % count;
		}
		else
		{
			CurrentTab = Mathf.Min(CurrentTab + 1, count - 1);
		}
	}

	public void UpdateInventories(List<IInventory> inventories)
	{
		while (_tabButtons.Count > inventories.Count)
		{
			var buttonToRemove = _tabButtons.Last();
			_tabButtons.Remove(buttonToRemove);
			_tabButtonContainer.RemoveChild(buttonToRemove);
			buttonToRemove.QueueFree();
		}

		// Add tabs based on the inventories list
		// while (_tabs.Count < inventories.Count)
		// {
		// 	var newTab = _inventoryViewPrefab.Instantiate<InventoryView>();
		// 	_tabContainer.AddChild(newTab);
		// 	_tabs.Add(newTab);
		// }

		while (_tabButtons.Count < inventories.Count)
		{
			var newButton = _inventoryCategoryTabPrefab.Instantiate<InventoryCategoryTab>();
			_tabButtonContainer.AddChild(newButton);
			_tabButtons.Add(newButton);
		}

		Assert.AssertEquals(inventories.Count, _tabButtons.Count, "Inventories and tabs count mismatch");

		for (int i = 0; i < inventories.Count; i++)
		{
			var inventory = inventories[i];

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

		_inventories = inventories;
		if (CurrentTab < 0 || CurrentTab >= inventories.Count)
		{
			CurrentTab = 0;
		}
		else
		{
			ForceUpdateView();
		}
	}

	public new void GrabFocus()
	{
		_inventoryView.GrabFocus();
	}

	private void ForceUpdateView()
	{
		try
		{
			var inventory = _inventories[CurrentTab];
			_inventoryView.UpdateInventory(inventory);
		} catch (ArgumentOutOfRangeException e)
		{
			GD.PrintErr("Inventory tab index out of range");
			// Log error
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
			CurrentTab = index;
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