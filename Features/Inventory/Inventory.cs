﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory;

public class Inventory : IInventory
{
	private readonly ItemStack[] _items;
	private readonly Logger _logger = new("Inventory");

	public int Size => _items.Length;
	public string Name { get; }

	public Inventory(int size, string name)
	{
		_items = new ItemStack[size];
		Name = name;
	}

	public ItemStack GetItem(int index)
	{
		return _items[index];
	}

	public void SetItem(int index, ItemStack item)
	{
		_items[index] = item;
	}

	public virtual Dictionary<int, ItemStack> AddItem(params ItemStack[] items)
	{
		Dictionary<int, ItemStack> overflow = new();
		for (var i = 0; i < items.Length; i++)
		{
			var leftover = AddItem(items[i]);
			if (leftover != null)
			{
				overflow.Add(i, leftover);
			}
		}

		return overflow;
	}

	public Dictionary<int, ItemStack> RemoveItem(params ItemStack[] items)
	{
		Dictionary<int, ItemStack> remainingToRemove = new();
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

	public List<ItemStack> GetItems()
	{
		return new List<ItemStack>(_items);
	}

	public void SetContents(List<ItemStack> items)
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
	}

	public bool Contains(string itemId)
	{
		return this.Any(stack => stack.Id == itemId);
	}

	public bool Contains(ItemStack item)
	{
		return this.Any(stack => stack.Id == item.Id);
	}

	public bool Contains(string itemId, int amount)
	{
		return this.Where(stack => stack.Id == itemId).Sum(stack => stack.Amount) >= amount;
	}

	public bool Contains(ItemStack item, int amount)
	{
		return this.Where(item.HasSameIdAndProps).Sum(stack => stack.Amount) >= amount;
	}

	public Dictionary<int, ItemStack> All(string itemId)
	{
		return this.Select((item, index) => (item, index))
			.Where(tuple => tuple.item.Id == itemId)
			.ToDictionary(tuple => tuple.index, tuple => tuple.item);
	}

	public Dictionary<int, ItemStack> All(ItemStack item)
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

	public int First(ItemStack item)
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
			if (_items[i].Id == itemId)
			{
				_items[i] = null;
			}
		}
	}

	public void RemoveAll(ItemStack item)
	{
		for (var i = 0; i < _items.Length; i++)
		{
			if (_items[i].HasSameIdAndProps(item))
			{
				_items[i] = null;
			}
		}
	}

	public void Clear(int index)
	{
		_items[index] = null;
	}

	public void Clear()
	{
		for (var i = 0; i < _items.Length; i++)
		{
			_items[i] = null;
		}
	}
	
	public Dictionary<int, ItemStack> GetItemsOfCategory(ItemCategory category)
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
	}

	public ItemStack AddItemToSlot(int slotIdx, ItemStack item)
	{
		if (slotIdx < 0 || slotIdx >= _items.Length)
		{
			_logger.Error("Invalid slot index");
			return item;
		}

		var existingItem = _items[slotIdx];
		if (existingItem == null)
		{
			_items[slotIdx] = item;
			return null;
		}

		// Check if stackable item
		if (!existingItem.HasSameIdAndProps(item))
		{
			return item;
		}

		var transferableAmount = existingItem.MaxStackSize - existingItem.Amount;
		if (transferableAmount >= item.Amount)
		{
			existingItem.Amount += item.Amount;
			return null;
		}

		var leftover = item.Clone() as ItemStack;
		existingItem.Amount += transferableAmount;
		leftover!.Amount -= transferableAmount;
		return leftover;
	}

	public IEnumerator<ItemStack> GetEnumerator()
	{
		return ((IEnumerable<ItemStack>)_items).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private ItemStack AddItem(ItemStack item)
	{
		if (item == null)
		{
			return null;
		}

		item = (item.Clone() as ItemStack)!;

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
			return item;
		}

		_items[emptyIdx] = item;
		return null;
	}

	private int GetFirstNonFull(ItemStack itemStack)
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

	private ItemStack RemoveItem(ItemStack item)
	{
		if (item == null)
		{
			return null;
		}

		item = item.Clone() as ItemStack;

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
				return null;
			}

			itemIndex = First(item.Id);
		}

		return item;
	}
}
