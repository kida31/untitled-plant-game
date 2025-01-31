﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory;

/// <summary>
///     Base implementation of an inventory.
/// </summary>
public class Inventory : IInventory
{
	public event Action InventoryChanged;
	public event Action<IItemStack> ItemAdded;
	public event Action<IItemStack> ItemRemoved;
	
	private readonly IItemStack[] _items;
	private readonly Logger _logger = new("Inventory");

	public int Size => _items.Length;
	public string Name { get; }

	public Inventory(int size, string name)
	{
		_items = new IItemStack[size];
		Name = name;
		
		ItemAdded += (_) => InventoryChanged?.Invoke();
		ItemRemoved += (_) => InventoryChanged?.Invoke();
	}

	public IItemStack GetItem(int index)
	{
		return _items[index];
	}

	public void SetItem(int index, IItemStack item)
	{
		_items[index] = item;
	}

	public virtual Dictionary<int, IItemStack> AddItem(params IItemStack[] items)
	{
		Dictionary<int, IItemStack> overflow = new();
		for (var i = 0; i < items.Length; i++)
		{
			var leftover = AddOneItem(items[i]);
			if (leftover != null)
			{
				overflow.Add(i, leftover);
			}
		}

		return overflow;
	}

	public Dictionary<int, IItemStack> RemoveItem(params IItemStack[] items)
	{
		Dictionary<int, IItemStack> remainingToRemove = new();
		for (var i = 0; i < items.Length; i++)
		{
			var leftover = RemoveItem(items[i]);
			if (leftover != null)
			{
				remainingToRemove.Add(i, leftover);
			}
		}

		return remainingToRemove;
	}

	public List<IItemStack> GetItems()
	{
		return new List<IItemStack>(_items);
	}

	public void SetContents(List<IItemStack> items)
	{
		if (items.Count > Size)
		{
			_logger.Error("Trying to set more items than inventory size");
			return;
		}

		for (var i = 0; i < _items.Length; i++)
		{
			_items[i] = i < items.Count ? items[i] : null;
		}
		
		InventoryChanged?.Invoke();
	}

	public bool Contains(string itemId)
	{
		return this.Any(stack => stack.Id == itemId);
	}

	public bool Contains(IItemStack item)
	{
		return item != null && this.Any(stack => stack?.Id == item.Id);
	}

	public bool Contains(string itemId, int amount)
	{
		return this.Where(stack => stack.Id == itemId).Sum(stack => stack.Amount) >= amount;
	}

	public bool Contains(IItemStack item, int amount)
	{
		return this.Where(item.HasSameIdAndProps).Sum(stack => stack.Amount) >= amount;
	}

	public Dictionary<int, IItemStack> All(string itemId)
	{
		return this.Select((item, index) => (item, index))
			.Where(tuple => tuple.item.Id == itemId)
			.ToDictionary(tuple => tuple.index, tuple => tuple.item);
	}

	public Dictionary<int, IItemStack> All(IItemStack item)
	{
		return this.Select((it, index) => (it, index))
			.Where(tuple => tuple.it.HasSameIdAndProps(item))
			.ToDictionary(tuple => tuple.index, tuple => tuple.it);
	}

	public int First(string itemId)
	{
		// return index of first item matching id else -1
		return Array.FindIndex(_items, item => item?.Id == itemId);
	}

	public int First(IItemStack item)
	{
		// return index of first item matching id else -1
		return Array.FindIndex(_items, it => it.HasSameIdAndProps(item));
	}

	protected int FirstEmpty()
	{
		return Array.FindIndex(_items, item => item == null);
	}

	public void RemoveAll(string itemId)
	{
		for (var i = 0; i < _items.Length; i++)
		{
			if (_items[i]?.Id == itemId)
			{
				_items[i] = null;
				ItemRemoved?.Invoke(_items[i]);
			}
		}
	}

	public void RemoveAll(IItemStack item)
	{
		for (var i = 0; i < _items.Length; i++)
		{
			if (_items[i]?.HasSameIdAndProps(item) ?? false)
			{
				_items[i] = null;
				ItemRemoved?.Invoke(_items[i]);
			}
		}
	}

	public void Clear(int index)
	{
		_items[index] = null;
		InventoryChanged?.Invoke();
	}

	public void Clear()
	{
		for (var i = 0; i < _items.Length; i++)
		{
			_items[i] = null;
		}
		InventoryChanged?.Invoke();
	}
	
	public Dictionary<int, IItemStack> GetItemsOfCategory(ItemCategory category)
	{
		return _items
			.Select((item, index) => (item, index))
			.Where(tuple => tuple.item.Category == category)
			.ToDictionary(tuple => tuple.index, tuple => tuple.item);
	}

	public void QuickStack(IInventory target)
	{
		for (var i = 0; i < _items.Length; i++)
		{
			// If the target inventory contains the item, add the item to the target inventory
			if (target.Contains(_items[i]))
			{
				var leftover = target.AddItem(_items[i]);
				_items[i] = leftover.ContainsKey(i) ? leftover[i] : null;
			}
		}
		InventoryChanged?.Invoke();
	}

	public IItemStack AddItemToSlot(int slotIdx, IItemStack item)
	{
		if (slotIdx < 0 || slotIdx >= _items.Length)
		{
			_logger.Error("Invalid slot index");
			return item;
		}

		// Empty slot
		var existingItem = _items[slotIdx];
		if (existingItem == null)
		{
			_items[slotIdx] = item;
			ItemAdded?.Invoke(item);
			return null;
		}

		// Slot has no stackable item, do nothing
		if (!existingItem.HasSameIdAndProps(item))
		{
			return item;
		}
		
		// Fully transfered
		var transferableAmount = existingItem.MaxStackSize - existingItem.Amount;
		if (transferableAmount >= item.Amount)
		{
			existingItem.Amount += item.Amount;
			ItemAdded?.Invoke(existingItem);
			return null;
		}

		// Partial transfer
		var leftover = item.Clone();
		existingItem.Amount += transferableAmount;
		leftover!.Amount -= transferableAmount;
		ItemAdded?.Invoke(item.Subtract(leftover));
		return leftover;
	}
	
	public IItemStack RemoveItemFromSlot(int slotIdx, IItemStack item)
	{
		if (slotIdx < 0 || slotIdx >= _items.Length)
		{
			_logger.Error("Invalid slot index");
			return item;
		}

		// Empty slot
		var existingItem = _items[slotIdx];
		if (existingItem == null)
		{
			_logger.Error("Trying to remove item from empty slot");
			return item;
		}

		// Slot does not have compatible item, do nothing
		if (!existingItem.HasSameIdAndProps(item))
		{
			_logger.Warn($"Items do not match. Expected {existingItem}, but got {item}");
			return item;
		}
		
		
		// Note: We do not remove partial stacks. This is a design decision.
		// If more items are requested than exist, we return with full item stack
		if (existingItem.Amount < item.Amount)
		{
			_logger.Warn("Tried to remove partial stack");
			return item;
		}
		
		// Full deduction
		existingItem.Amount -= item.Amount;
		if (existingItem.Amount == 0) // Checking <= 0 would be a better fallback, but would hide potential bugs
		{
			_items[slotIdx] = null;
		}

		ItemRemoved?.Invoke(item);
		return null;
	}

	public IItemStack PopItemFromSlot(int slotIdx, int amount)
	{
		if (slotIdx < 0 || slotIdx >= _items.Length)
		{
			_logger.Error("Invalid slot index");
			return null;
		}

		// Empty slot
		var existingItem = _items[slotIdx];
		if (existingItem == null)
		{
			_logger.Error("Trying to remove item from empty slot");
			return null;
		}
		
		// Note: We do not remove partial stacks. This is a design decision.
		// If more items are requested than exist, we return with full item stack
		if (existingItem.Amount < amount)
		{
			return null;
		}
		
		// Full deduction
		var poppedItem = existingItem.Clone();
		poppedItem.Amount = amount;
		
		existingItem.Amount -= amount;
		if (existingItem.Amount == 0) // Checking <= 0 would be a better fallback, but would hide potential bugs
		{
			_items[slotIdx] = null;
		}

		ItemRemoved?.Invoke(poppedItem);
		return poppedItem;
	}

	public IEnumerator<IItemStack> GetEnumerator()
	{
		return ((IEnumerable<IItemStack>)_items).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private IItemStack AddOneItem(IItemStack item)
	{
		_logger.Debug("Adding one item stack " + item);
		if (item == null)
		{
			return null;
		}

		var originalItem = item.Clone();
		item = item.Clone();

		// Try to fill up existing item stacks
		var nonFullStackIdx = GetFirstNonFull(item);
		while (nonFullStackIdx != -1)
		{
			var destination = _items[nonFullStackIdx];
			var transferableAmount = destination.MaxStackSize - destination.Amount;

			// If remaining amount fits into the stack, we are done
			if (transferableAmount >= item.Amount)
			{
				destination.Amount += item.Amount;
				item.Amount = 0;
				_logger.Debug("Added item to existing stack");
				ItemAdded?.Invoke(originalItem);
				return null;
			}

			// Otherwise fill up and try to fill up another stack
			destination.Amount = destination.MaxStackSize;
			item.Amount -= transferableAmount;
			nonFullStackIdx = GetFirstNonFull(item);
		}

		// Try to add remaining items to empty slots
		var emptyIdx = FirstEmpty();
		if (emptyIdx == -1)
		{
			_logger.Debug("Inventory full, could not add item completely");
			ItemAdded?.Invoke(originalItem.Subtract(item));
			return item;
		}

		_items[emptyIdx] = item;
		_logger.Debug("Added item to empty slot");
		ItemAdded?.Invoke(originalItem);
		return null;
	}

	private int GetFirstNonFull(IItemStack itemStack)
	{
		for (var i = 0; i < _items.Length; i++)
		{
			if (itemStack.HasSameIdAndProps(_items[i]) && _items[i].Amount < _items[i].MaxStackSize)
			{
				return i;
			}
		}

		return -1;
	}

	private IItemStack RemoveItem(IItemStack item)
	{
		if (item == null)
		{
			return null;
		}

		var originalItem = item.Clone();
		item = item.Clone();

		var itemIndex = First(item!.Id);
		while (itemIndex != -1)
		{
			var sourceItem = _items[itemIndex];
			// Remove as much as possible
			var deducibleAmount = Math.Min(sourceItem.Amount, item.Amount);
			if (deducibleAmount < sourceItem.Amount)
			{
				sourceItem.Amount -= deducibleAmount;
			}
			else
			{
				_items[itemIndex] = null; // Removed all of the item
			}

			item.Amount -= deducibleAmount;
			if (item.Amount == 0)
			{
				ItemRemoved?.Invoke(originalItem);
				return null;
			}

			itemIndex = First(item.Id);
		}

		ItemRemoved?.Invoke(originalItem.Subtract(item));
		return item;
	}
}
