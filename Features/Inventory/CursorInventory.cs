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

/// <summary>
///     Represents a cursor and its inventory (one item stack). This is used when navigating the inventory
///     and moving around items.
/// </summary>
[Singleton]
public class CursorInventory : ICursorInventory
{
	// Singleton (keep this at the top)
	private static readonly Lazy<CursorInventory> LazySingleton = new(() => new CursorInventory());
	public static CursorInventory Instance => LazySingleton.Value;
	public event Action ContentChanged;

	/// <summary>
	///     Invoked when an item is orphaned during any process. Game should drop the item to the ground.
	/// </summary>
	public event Action<IItemStack> ItemOrphaned;

	public IItemStack Content => _content;


	private IItemStack _content;
	private readonly Logger _logger;
	private IInventory _mostRecentInventoryPickup;

	private CursorInventory()
	{
		_logger = new(GetType().Name);
		_mostRecentInventoryPickup = null;
		_content = null;
	}

	public void HandleClick(IInventory inventory, int itemIndex)
	{
		Assert.AssertTrue(CanClick(inventory, itemIndex),
			"Cursor could not handle click. Doing nothing. Consider using 'CanClick' before invoking this");

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
	}

	/// <summary>
	///     Handles secondary transaction/feature on some other inventory
	///     If cursor has content and target inventory has item, it will try to take one item if stackable
	///     If cursor has no item and target inventory has item, it will try to take one item.
	///     If cursor has content and target inventory has no empty, it will try to put down one item.
	///     If cursor has no item and target inventory has no item, it will do nothing.
	/// </summary>
	/// <param name="inventory"></param>
	/// <param name="itemIndex"></param>
	public void HandleSecondary(IInventory inventory, int itemIndex)
	{
		var targetItem = inventory.GetItem(itemIndex);

		// Empty Hand, Empty Slot
		// RMB - Nothing
		if (_content == null && targetItem == null)
		{
			return;
		}

		// 50 Hand, Empty Slot
		// RMB - Drop 1
		if (_content != null && targetItem == null)
		{
			TryDropOne();
			return;
		}

		// Empty Hand, 50 Slot
		// RMB - Take 1
		if (_content == null && targetItem != null)
		{
			TryTakeOne();
			return;
		}

		// 50 Hand, 50 Slot
		// RMB - Take 1 (stack) or nothing
		if (_content != null && targetItem != null)
		{
			TryTakeOne();
			return;
		}

		_logger.Debug("Secondary -> Nothing to do here");
		return;

		// Helpers
		void TryDropOne()
		{
			var addedItem = _content.Clone();
			addedItem.Amount = 1;

			var remainder = inventory.AddItemToSlot(itemIndex, addedItem);
			if (remainder != null)
			{
				_logger.Error("Failed to put down item");
				return;
			}

			_content.Amount -= 1;
			if (_content.Amount == 0)
			{
				ClearContent();
			}

			ContentChanged?.Invoke();
		}

		void TryTakeOne()
		{
			// !(is stackable in hand or hand is empty)
			if (_content != null && !_content.HasSameIdAndProps(targetItem))
			{
				// do nothing (could consider swapping)
				return;
			}
			
			var itemStack = inventory.PopItemFromSlot(itemIndex, 1);
			if (itemStack == null)
			{
				_logger.Error("Failed to pick up item");
				return;
			}

			if (_content == null)
			{
				_content = itemStack;
			}
			else
			{
				_content.Amount += 1;
			}
			_mostRecentInventoryPickup = inventory;
			ContentChanged?.Invoke();
		}
	}

	public bool CanClick(IInventory inventory, int itemIndex)
	{
		return new List<Func<IInventory, int, bool>> { CanPickUp, CanPutDown, CanStack, CanSwap }.Any(f => f(inventory, itemIndex));
	}

	public bool CanPickUp(IInventory inventory, int itemIndex)
	{
		return _content == null && inventory.GetItem(itemIndex) != null;
	}

	public void PickUp(IInventory inventory, int itemIndex)
	{
		var item = inventory.GetItem(itemIndex).Clone();
		if (item == null)
		{
			_logger.Error("");
			return;
		}

		var result = inventory.RemoveItemFromSlot(itemIndex, item);
		if (result != null)
		{
			_logger.Error("");
			return;
		}

		_content = item;
		_mostRecentInventoryPickup = inventory;
		ContentChanged?.Invoke();
	}


	public bool CanPutDown(IInventory inventory, int itemIndex)
	{
		return _content != null && inventory.GetItem(itemIndex) == null;
	}

	public void PutDown(IInventory inventory, int itemIndex)
	{
		var remainder = inventory.AddItemToSlot(itemIndex, _content);
		if (remainder != null)
		{
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
		_logger.Debug($"{_content} <=> {inventory.GetItem(itemIndex)}");
		return _content != null && inventory.GetItem(itemIndex) != null;
	}

	public void Swap(IInventory inventory, int itemIndex)
	{
		var itemAtDestination = inventory.GetItem(itemIndex)?.Clone();
		if (itemAtDestination == null || _content == null)
		{
			_logger.Error("Nothing to swap here");
			return;
		}

		var result = inventory.RemoveItemFromSlot(itemIndex, itemAtDestination);
		if (result != null)
		{
			_logger.Error("Could not get item out of target inventory");
			return;
		}

		var remainder = inventory.AddItemToSlot(itemIndex, _content);
		if (remainder != null)
		{
			_logger.Error("Failed to put content into inventory during swap.");
			ItemOrphaned?.Invoke(_content);
		}

		_content = itemAtDestination;
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

		if (_mostRecentInventoryPickup == null)
		{
			_logger.Warn("No origin location saved to return item to");
			ItemOrphaned?.Invoke(_content);
		}

		var remainder = _mostRecentInventoryPickup?.AddItem(_content);

		if (remainder != null && remainder.Count > 0)
		{
			// This might be the case with items that have special inventory restrictions or space was occupied somehow
			_logger.Error("Could not put item back to origin although destination is free.");
			ItemOrphaned?.Invoke(_content);
		}

		ClearContent();
		_logger.Info("Returned item back to origin");
	}

	private void ClearContent()
	{
		_content = null;
		_mostRecentInventoryPickup = null;
		ContentChanged?.Invoke();
	}
}
