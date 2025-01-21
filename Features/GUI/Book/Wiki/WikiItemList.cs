using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Book.Wiki;

/// <summary>
///     This node is a control that displays a list of items in the wiki.
/// </summary>
public partial class WikiItemList : Control
{
	[Export] private VBoxContainer _itemViewContainer;

	[Export] private PackedScene _itemViewPrefab;

	private List<WikiItemView> _itemViews;
	[Export] private Button _materialButton;
	[Export] private Button _medicineButton;
	[Export] private Button _otherButton;

	[ExportCategory("SectionButtons")] [Export] private Button _plantButton;
	public event Action<IItemStack> ItemStackPressed; // TODO: Use local events instead of event bus where possible

	public override void _Ready()
	{
		// Initialize list
		_itemViews = new List<WikiItemView>();

		// Removes placeholders
		_itemViewContainer.GetChildren().ToList().ForEach(c => c.Free());

		// Connect inputs
		// TODO this could be delegated to upper layer
		_plantButton.Pressed += () => ScrollToFirstItemOf(ItemCategory.Seed);
		_materialButton.Pressed += () => ScrollToFirstItemOf(ItemCategory.Material);
		_medicineButton.Pressed += () => ScrollToFirstItemOf(ItemCategory.Medicine);
	}
	
	public void SetItems(List<IItemStack> items)
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

	public new void GrabFocus()
	{
		_itemViews.FirstOrDefault()?.GrabFocus();
	}

	// Temporary solution
	private void ScrollToFirstItemOf(ItemCategory category)
	{
		var item = _itemViews.Select(iv => iv.ItemStack).FirstOrDefault(its => its.Category == category);
		if (item != null)
		{
			ScrollTo(item);
		}
	}

	public void ScrollTo(IItemStack itemStack)
	{
		var itemView = _itemViews.FirstOrDefault(iv => iv.ItemStack?.HasSameId(itemStack) ?? false);
		Assert.AssertNotNull(itemView, "Item not found in list: " + itemStack);
		itemView?.GrabFocus();
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
