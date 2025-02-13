﻿using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Book.Inventories;

/// <summary>
///     This class is a view for the inventory. It displays the items in the inventory and allows the player to interact with them.
/// </summary>
public partial class InventoryView : Control
{
	private readonly Dictionary<InventoryItemView, Action> _itemViewPressedActions = new();
	private readonly Dictionary<InventoryItemView, Action> _itemViewSecondaryPressedActions = new();
	[Export] private Container _inventoryItemViewContainer;
	[Export] private PackedScene _inventoryItemViewPrefab;

	private List<InventoryItemView> _inventoryItemViews;
	[Export] private Label _itemNameLabel;

	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		_inventoryItemViews = _inventoryItemViewContainer.GetChildren().OfType<InventoryItemView>().ToList();
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
		_inventoryItemViews = _inventoryItemViewContainer.GetChildren().OfType<InventoryItemView>().ToList();

		var items = inventory.GetItems();
		FillTabWithEmptyInventoryItemViews(items.Count);

		// Populate content of the item views
		for (var i = 0; i < _inventoryItemViews.Count; i++)
		{
			var view = _inventoryItemViews[i];
			view.UpdateItemView(i < items.Count ? items[i] : null);

			// Update actions
			if (_itemViewPressedActions.ContainsKey(view))
			{
				// unbind LMB
				view.Pressed -= _itemViewPressedActions[view];
				// unbind RMB
				view.SecondaryPressed -= _itemViewSecondaryPressedActions[view];
			}

			if (i >= items.Count)
			{
				continue;
			}

			var snapshotIndex = i;

			void PressedHandler()
			{
				_logger.Debug($"Handle click on {inventory.Name}[{snapshotIndex}]");
				if (CursorInventory.Instance.CanClick(inventory, snapshotIndex))
				{
					CursorInventory.Instance.HandleClick(inventory, snapshotIndex);
				}
			}

			void SecondaryPressedHandler()
			{
				_logger.Debug($"Handle secondary click on {inventory.Name}[{snapshotIndex}]");
				CursorInventory.Instance.HandleSecondary(inventory, snapshotIndex);
			}

			// Bind LMB
			_itemViewPressedActions[view] = PressedHandler;
			view.Pressed += PressedHandler;
			// Bind RMB
			_itemViewSecondaryPressedActions[view] = SecondaryPressedHandler;
			view.SecondaryPressed += SecondaryPressedHandler;
		}

		// I dont know where to place this

		// Update item label depending on currently focused item view
		var owner = GetViewport().GuiGetFocusOwner();
		if (owner is InventoryItemView itemView)
		{
			OnItemViewFocused(itemView);
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
			itemView.FocusEntered += () => OnItemViewFocused(itemView);
			_inventoryItemViews.Add(itemView);
		}
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
