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
	[Export] private Label _itemNameLabel;

	private List<InventoryItemView> _inventoryItemViews;

	private Dictionary<InventoryItemView, Action> _itemViewPressedActions = new();

	public override void _Ready()
	{

		_inventoryItemViews = _inventoryItemViewContainer.GetChildren().OfType<InventoryItemView>().ToList();
		_inventoryItemViews.ForEach(iv =>
		{
			iv.FocusEntered += () => OnItemViewFocused(iv);
		});
		_itemNameLabel.Text = "";
	}

	public void UpdateInventory(IInventory inventory)
	{
		_inventoryItemViews = _inventoryItemViewContainer.GetChildren().OfType<InventoryItemView>().ToList();

		var items = inventory.GetItems();
		FillTabWithEmptyInventoryItemViews(items.Count);

		// Populate content of the item views
		for (int i = 0; i < _inventoryItemViews.Count; i++)
		{
			var view = _inventoryItemViews[i];
			view.UpdateItemView(i < items.Count ? items[i] : null);

			// Update actions
			if (_itemViewPressedActions.ContainsKey(view))
			{
				view.Pressed -= _itemViewPressedActions[view];
			}

			if (i < items.Count)
			{
				var curratedIndex = i;
				void PressedHandler()
				{
					// GD.Print($"Handle click on {inventory.Name}[{curratedIndex}/{i}]");
					CursorInventory.Instance.HandleClick(inventory, curratedIndex);
				}

				_itemViewPressedActions[view] = PressedHandler;
				view.Pressed += PressedHandler;
			}
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
			itemView.FocusEntered += () => OnItemViewFocused(itemView);
			_inventoryItemViews.Add(itemView);
		}
	}

	private void OnItemViewFocused(InventoryItemView itemView)
	{
		_itemNameLabel.Text = itemView.ItemStack?.Name ?? "";
		// GD.Print("Focused" + itemView.Name);
	}
}
