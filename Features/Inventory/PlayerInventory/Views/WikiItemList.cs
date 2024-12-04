using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.PlayerInventory.UI_Wiki;
using untitledplantgame.Shops;

public partial class WikiItemList : PanelContainer
{
	public event Action<ItemStack> ItemStackPressed; // TODO: Use local events instead of event bus where possible

	[Export] private PackedScene _itemViewPrefab;
	[Export] private VBoxContainer _itemViewContainer;

	public override void _Ready()
	{
		// Initialize list
		_itemViews = new();

		// Removes placeholders
		_itemViewContainer.GetChildren().ToList().ForEach(c => c.Free());
	}

	private List<WikiItemView> _itemViews;

	public void SetItems(List<ItemStack> items)
	{
		Assert.AssertTrue(_itemViews.Count == _itemViewContainer.GetChildCount(), "Tracked views and actual are not equal");

		// Remove nodes if too many
		while (_itemViews.Count > items.Count)
		{
			var itemView = _itemViews[0];
			_itemViews.RemoveAt(0);
			DisconnectItemView(itemView);
		}

		// Add nodes if too few
		while (_itemViews.Count < items.Count)
		{
			var itemView = _itemViewPrefab.Instantiate<WikiItemView>();
			_itemViews.Add(itemView);
			ConnectItemView(itemView);
		}

		// Fill with content
		Assert.AssertTrue(_itemViews.Count == items.Count, "Items and views are not equal count");
		for (var index = 0; index < items.Count; index++)
		{
			var item = items[index];
			_itemViews[index].ItemStack = item; // keep this in two lines for debugging
		}
	}

	public void ScrollTo(ItemStack itemStack)
	{
		// TODO: 
	}

	private void ConnectItemView(WikiItemView itemView)
	{
		itemView.FocusEntered += () => ItemStackPressed?.Invoke(itemView.ItemStack);
		itemView.Pressed += () => ItemStackPressed?.Invoke(itemView.ItemStack);
		_itemViewContainer.AddChild(itemView);
	}
	
	private void DisconnectItemView(WikiItemView itemView)
	{
		// Do not need to unsubscribe since object is being removed
		RemoveChild(itemView);
		itemView.QueueFree();
	}
}
