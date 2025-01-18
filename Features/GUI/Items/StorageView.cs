using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.GUI.Book.Inventories;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Items;

/// <summary>
///     This class is a view for simple storages. Storages are any contains that can hold items.
///		This view displays the contents of a storage and allows organizing items (picking up and dropping items)
/// </summary>
public partial class StorageView : Control
{
	// Setup
	[Export] private PackedScene _itemViewPrefab;
	[Export] private Container _itemViewContainer;
	[Export] private Label _itemNameLabel;

	private List<NewInventoryItemView> _itemViews;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);

		_itemViews = _itemViewContainer.GetChildren().OfType<NewInventoryItemView>().ToList();
		_itemViews.ForEach(iv => { iv.FocusEntered += () => UpdateSelectedItemLabel(iv); });
		if (_itemNameLabel != null) _itemNameLabel.Text = "";

		VisibilityChanged += () =>
		{
			if (!IsVisibleInTree())
			{
				OnVisibilityOff();
			}
		};
	}

	public void ShowInventory(IInventory inventory)
	{
		_itemViews = _itemViewContainer.GetChildren().OfType<NewInventoryItemView>().ToList();

		var items = inventory.GetItems();
		PrepareItemViewNodes(items.Count);

		// Hook up item views
		for (var i = 0; i < _itemViews.Count; i++)
		{
			var itemView = _itemViews[i];

			itemView.Inventory = inventory;
			itemView.SlotIndex = i < items.Count ? i : -1;
		}

		// Update item label
		var owner = GetViewport().GuiGetFocusOwner();
		if (owner is NewInventoryItemView iv)
		{
			UpdateSelectedItemLabel(iv);
		}
	}

	/// <summary>
	///     Focuses the first item view
	/// </summary>
	public new void GrabFocus()
	{
		_itemViews.FirstOrDefault()?.GrabFocus();
	}

	/// <summary>
	///     Removes or adds item views to match the new item count
	/// </summary>
	/// <param name="amount"></param>
	private void PrepareItemViewNodes(int amount)
	{
		if (_itemViews.Count != _itemViewContainer.GetChildCount())
		{
			_logger.Warn("Number of items did not match. " +
			             $"There are {_itemViews.Count} tracked item views, but container has {_itemViewContainer.GetChildCount()} children.");
		}

		// Remove extra item views...
		while (_itemViews.Count > amount)
		{
			var itemView = _itemViews[0];
			_itemViews.Remove(itemView);
			itemView.QueueFree();
			//  We do not need to unsubscribe since object is being freed.
		}

		// ...or add new ones
		while (_itemViews.Count < amount)
		{
			var itemView = _itemViewPrefab.Instantiate<NewInventoryItemView>();
			_itemViewContainer.AddChild(itemView);
			itemView.FocusEntered += () => UpdateSelectedItemLabel(itemView);
			_itemViews.Add(itemView);
		}

		Assert.AssertTrue(_itemViews.Count == _itemViewContainer.GetChildCount(),
			$"Number of items did not match after adjustment. Tracked nodes are {_itemViews.Count} and container children are {_itemViewContainer.GetChildCount()}");
	}

	private void UpdateSelectedItemLabel(NewInventoryItemView itemView)
	{
		if (_itemNameLabel == null) return;
		_itemNameLabel.Text = itemView.ItemStack?.Name ?? "";
	}

	private void OnVisibilityOff()
	{
		// TODO: It is not clear that this needs to be done.
		CursorInventory.Instance.ReturnPickUp(); // Returns any items currently in hand. "Cancels transaction"
	}
}
