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
		
		// Remove placeholders
		_itemViewContainer.GetChildren().ToList().ForEach(c => c.Free());

		// Examples TODO: Remove this.
		var items = new RandomStockGenerator().GetRandom(20);
		SetItems(items);
	}

	private List<WikiItemView> _itemViews;

	public void SetItems(List<ItemStack> items)
	{
		Assert.AssertTrue(_itemViews.Count == _itemViewContainer.GetChildCount(), "Tracked views and actual are not equal");

		// Initiate nodes
		while (_itemViews.Count > items.Count)
		{
			var itemView = _itemViews[0];
			_itemViews.RemoveAt(0);
			RemoveChild(itemView);
			itemView.QueueFree();
		}

		while (_itemViews.Count < items.Count)
		{
			var itemView = _itemViewPrefab.Instantiate<WikiItemView>();
			_itemViews.Add(itemView);
			itemView.FocusEntered += () => ItemStackPressed?.Invoke(itemView.ItemStack);
			itemView.Pressed += () =>
			{
				GD.Print("Pressed item view.");
				ItemStackPressed?.Invoke(itemView.ItemStack);
			};
			_itemViewContainer.AddChild(itemView);
		}

		Assert.AssertTrue(_itemViews.Count == items.Count, "Items and views are not equal count");
		// Fill with content
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
}
