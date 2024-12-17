using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

public partial class WikiItemList : Control
{
	public event Action<IItemStack> ItemStackPressed; // TODO: Use local events instead of event bus where possible
	/*
	public event Action PlantButtonPressed;
	public event Action MaterialButtonPressed;
	public event Action MedicineButtonPressed;
	public event Action OtherButtonPressed;
	*/
	
	[Export] private PackedScene _itemViewPrefab;
	[Export] private VBoxContainer _itemViewContainer;
	
	[ExportCategory("SectionButtons")]
	[Export] private Button _plantButton;
	[Export] private Button _materialButton;
	[Export] private Button _medicineButton;
	[Export] private Button _otherButton;
	
	private List<WikiItemView> _itemViews;
	
	public override void _Ready()
	{
		// Initialize list
		_itemViews = new();

		// Removes placeholders
		_itemViewContainer.GetChildren().ToList().ForEach(c => c.Free());
		
		// TODO this could be delegated to upper layer
		/*
		_plantButton.Pressed += () => PlantButtonPressed?.Invoke();
		_materialButton.Pressed += () => MaterialButtonPressed?.Invoke();
		_medicineButton.Pressed += () => MedicineButtonPressed?.Invoke();
		_otherButton.Pressed += () => OtherButtonPressed?.Invoke();
		*/

		_plantButton.Pressed += () => ScrollToFirstItemOf(ItemCategory.Plant);
		_materialButton.Pressed += () => ScrollToFirstItemOf(ItemCategory.Material);
		_medicineButton.Pressed += () => ScrollToFirstItemOf(ItemCategory.Medicine);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsKeyPressed(Key.F1))
		{
			_itemViews[^1].GrabFocus();
		}
	}
	
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
		var itemView = _itemViews.FirstOrDefault(iv => iv.ItemStack == itemStack);
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
