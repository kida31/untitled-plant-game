using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Book.Inventories;

/// <summary>
///     This class is a view for the inventory tabs. It displays the tabs for the different inventories and a single view
///     for the currently selected inventory. The view updates depending on the selected tab.
/// </summary>
public partial class InventoryTabsView : Control
{
	// private List<InventoryView> _tabs;

	private readonly Dictionary<InventoryCategoryTab, Action> _buttonActions = new();

	private int _currentTab = -1;

	private List<IInventory> _inventories = new();

	[Export] private PackedScene _inventoryCategoryTabPrefab;
	[Export] private InventoryView _inventoryView;

	[Export] private Container _tabButtonContainer;

	private List<InventoryCategoryTab> _tabButtons;

	private int CurrentTab
	{
		get => _currentTab;
		set
		{
			_currentTab = Math.Clamp(value, 0, _inventories.Count);
			TabChanged?.Invoke(_currentTab);
		}
	}

	public event Action<long> TabChanged;

	public override void _Ready()
	{
		_tabButtons = _tabButtonContainer.GetChildren().OfType<InventoryCategoryTab>().ToList();

		TabChanged += OnTabChanged;
		TabChanged += _ => CursorInventory.Instance.ReturnPickUp();
	}

	private void OnTabChanged(long tab)
	{
		var activeIndex = (int) tab;
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

		if (@event.IsActionPressed(Common.Inputs.GameActions.Book.BumperLeft))
		{
			PreviousTab(false);
		}
		else if (@event.IsActionPressed(Common.Inputs.GameActions.Book.BumperRight))
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

		for (var i = 0; i < inventories.Count; i++)
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

			void ButtonAction()
			{
				OnTabButtonPressed(button);
			}

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
		}
		catch (ArgumentOutOfRangeException e)
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
}
