using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory;

/// <summary>
/// Represents a cursor and its inventory (one item stack)
/// </summary>
interface ICursorInventory
{
	/// <summary>
	/// Invoked when an item is orphaned during any process.
	/// During invalid operations,y when inventories reject the item or it simply has on place to return to.
	/// </summary>
	event Action<IItemStack> ItemOrphaned;

	/// <summary>
	/// Invoked when the content of cursor inventory changes.
	/// </summary>
	event Action ContentChanged;

	/// <summary>
	/// The current item stack inside the cursor inventory
	/// </summary>
	IItemStack Content { get; }

	/// <summary>
	/// Returns whether a "click" operation would be valid.
	/// </summary>
	/// <param name="inventory"></param>
	/// <param name="itemIndex"></param>
	/// <returns></returns>
	bool CanClick(IInventory inventory, int itemIndex);

	/// <summary>
	/// Handles a click. May pickup, put down, stack or swap items depending on context.
	/// </summary>
	/// <param name="inventory"></param>
	/// <param name="itemIndex"></param>
	void HandleClick(IInventory inventory, int itemIndex);

	/// <summary>
	/// Returns whether an item can be picked up. False if target has no item or cursor already has content
	/// </summary>
	/// <param name="inventory"></param>
	/// <param name="itemIndex"></param>
	/// <returns></returns>
	bool CanPickUp(IInventory inventory, int itemIndex);
	/// <summary>
	/// Adds the targeted item to cursor content.
	/// </summary>
	/// <param name="inventory"></param>
	/// <param name="itemIndex"></param>
	void PickUp(IInventory inventory, int itemIndex);

	/// <summary>
	/// Returns whether an item can be put down. False if destination already has item or there is no content;
	/// </summary>
	/// <param name="inventory"></param>
	/// <param name="itemIndex"></param>
	/// <returns></returns>
	bool CanPutDown(IInventory inventory, int itemIndex);

	/// <summary>
	/// Puts the cursor content into the target inventory slot
	/// </summary>
	/// <param name="inventory"></param>
	/// <param name="itemIndex"></param>
	void PutDown(IInventory inventory, int itemIndex);

	/// <summary>
	/// Returns whether the cursor content can stack with target inventory
	/// </summary>
	/// <param name="inventory"></param>
	/// <param name="itemIndex"></param>
	/// <returns></returns>
	bool CanStack(IInventory inventory, int itemIndex);

	/// <summary>
	/// Stacks the cursor content onto the target inventory slot. The content will be updated to the remainder.
	/// </summary>
	/// <param name="inventory"></param>
	/// <param name="itemIndex"></param>
	void Stack(IInventory inventory, int itemIndex);

	/// <summary>
	/// Returns whether content can be swapped with target inventory slot
	/// </summary>
	/// <param name="inventory"></param>
	/// <param name="itemIndex"></param>
	/// <returns></returns>
	bool CanSwap(IInventory inventory, int itemIndex);

	/// <summary>
	/// Swaps cursor content with target inventory item slot. Will invoke ItemOrphaned if content cannot be placed back into inventory.
	/// </summary>
	/// <param name="inventory"></param>
	/// <param name="itemIndex"></param>
	void Swap(IInventory inventory, int itemIndex);

	/// <summary>
	/// Tries to return the item content to its first pick up origin. Will invoke ItemOrphaned if not possible.
	/// </summary>
	void ReturnPickUp();
}

[Singleton]
public class CursorInventory : ICursorInventory
{
	// Singleton (keep this at the top)
	private static readonly Lazy<CursorInventory> LazySingleton = new(() => new CursorInventory());
	public static CursorInventory Instance => LazySingleton.Value;
	public event Action ContentChanged;
	public event Action<IItemStack> ItemOrphaned;

	public IItemStack Content => _content;


	private IItemStack _content;
	private readonly Logger _logger;
	private IInventory _pickupOrigin;
	private int _pickupOriginIndex;

	private CursorInventory()
	{
		_logger = new(GetType().Name);
		_pickupOrigin = null;
		_pickupOriginIndex = -1;
		_content = null;
	}

	public void HandleClick(IInventory inventory, int itemIndex)
	{
		Assert.AssertTrue(CanClick(inventory, itemIndex));

		if (CanPickUp(inventory, itemIndex))
		{
			PickUp(inventory, itemIndex);
			return;
		}

		if (CanPutDown(inventory, itemIndex))
		{
			PutDown(inventory, itemIndex);
			return;
		}

		if (CanStack(inventory, itemIndex))
		{
			Stack(inventory, itemIndex);
			return;
		}

		if (CanSwap(inventory, itemIndex))
		{
			Swap(inventory, itemIndex);
			return;
		}

		_logger.Warn("Cursor could not handle click. Doing nothing. Consider using 'CanClick' before invoking this");
	}

	public bool CanClick(IInventory inventory, int itemIndex)
	{
		return new List<Func<IInventory, int, bool>>() { CanPickUp, CanPutDown, CanStack, CanSwap }.Any(f => f(inventory, itemIndex));
	}

	public bool CanPickUp(IInventory inventory, int itemIndex)
	{
		return _content == null && inventory.GetItem(itemIndex) != null;
	}

	public void PickUp(IInventory inventory, int itemIndex)
	{
		var item = inventory.GetItem(itemIndex).Clone();
		if (item == null) {
			_logger.Error("");
			return;
		}

		var result = inventory.RemoveItem(item);
		if (result.Count != 0) {
			_logger.Error("");
			return;
		}


		_content = item;
		_pickupOrigin = inventory;
		_pickupOriginIndex = itemIndex;
		ContentChanged?.Invoke();
	}


	public bool CanPutDown(IInventory inventory, int itemIndex)
	{
		return _content != null && inventory.GetItem(itemIndex) == null;
	}

	public void PutDown(IInventory inventory, int itemIndex)
	{
		var remainder = inventory.AddItemToSlot(itemIndex, _content);
		if (remainder != null) {
			// dupe exploit lol
			_logger.Error("Failed to put down item");
			return;
		}
		ClearContent();
	}

	public bool CanStack(IInventory inventory, int itemIndex)
	{
		return _content != null && _content.HasSameIdAndProps(inventory.GetItem(itemIndex));
	}

	public void Stack(IInventory inventory, int itemIndex)
	{
		_content = inventory.AddItemToSlot(itemIndex, _content);
		ContentChanged?.Invoke();
	}

	public bool CanSwap(IInventory inventory, int itemIndex)
	{
		return true; // I don't know why this would not work
	}

	public void Swap(IInventory inventory, int itemIndex)
	{
		var item = inventory.GetItem(itemIndex)?.Clone();
		if (item == null || _content == null) {
			_logger.Error("Nothing to swap here");
			return;
		}

		var result = inventory.RemoveItem(item);
		if (result.Count > 0) {
			_logger.Error("Could not get item out of target inventory");
			return;
		}

		var remainder = inventory.AddItemToSlot(itemIndex, _content);
		if (remainder != null) {
			_logger.Error("Failed to put content into inventory during swap.");
			ItemOrphaned?.Invoke(_content);
		}

		_content = item;
		_logger.Info("Swapped item");
		ContentChanged?.Invoke();
	}

	public void ReturnPickUp()
	{
		if (_content == null)
		{
			_logger.Info("There is no item to return");
			return;
		}

		if (_pickupOrigin == null || _pickupOriginIndex < 0)
		{
			_logger.Warn("No origin location saved to return item to");
			ItemOrphaned?.Invoke(_content);
		}

		var destination = _pickupOrigin?.GetItem(_pickupOriginIndex);

		if (destination != null)
		{
			_logger.Error("Original space is occupied");
			ItemOrphaned?.Invoke(_content);
		}

		var remainder = _pickupOrigin.AddItemToSlot(_pickupOriginIndex, _content);

		if (remainder != null)
		{
			// This might be the case with items that have special inventory restrictions
			_logger.Error("Could not put item back to origin although destination is free. Unexpected Error");
			ItemOrphaned?.Invoke(_content);
		}

		ClearContent();
		_logger.Info("Returned item back to origin");
	}

	private void ClearContent() {
		_content = null;
		_pickupOrigin = null;
		_pickupOriginIndex = -1;
		ContentChanged?.Invoke();
	}
}
