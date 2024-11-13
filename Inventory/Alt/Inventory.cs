using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace untitledplantgame.Inventory.Alt;

public class Inventory : IInventory
{
	public int Size => _items.Length;
	public int MaxStackSize { get; set; }
	public string Name { get; }

	private ItemStack[] _items;

	public Inventory(int size, int maxStackSize, string name)
	{
		_items = new ItemStack[size];
		MaxStackSize = maxStackSize;
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

	private ItemStack AddItem(ItemStack item)
	{
		if (item == null)
		{
			return null;
		}

		item = item.Clone() as ItemStack;

		while (true)
		{
			var idx = GetFirstNonFull(item!.Id);
			if (idx == -1)
			{
				break;
			}

			var destination = _items[idx];
			var transferableAmount = MaxStackSize - destination.Amount;
			if (transferableAmount < item.Amount)
			{
				destination.Amount = MaxStackSize;
				item.Amount -= transferableAmount;
			}
			else
			{
				destination.Amount += item.Amount;
				item.Amount = 0;
				return null;
			}
		}

		var emptyIdx = FirstEmpty();
		if (emptyIdx == -1)
		{
			return item;
		}

		_items[emptyIdx] = item;
		return null;
	}

	private int GetFirstNonFull(string itemId)
	{
		for (var i = 0; i < _items.Length; i++)
		{
			if (_items[i].Id == itemId && _items[i].Amount < _items[i].MaxStackSize)
			{
				return i;
			}
		}

		return -1;
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

	public List<ItemStack> GetContents()
	{
		return new List<ItemStack>(_items);
	}

	public void SetContents(List<ItemStack> items)
	{
		throw new NotImplementedException();
	}

	public bool Contains(string itemId)
	{
		throw new NotImplementedException();
	}

	public bool Contains(ItemStack item)
	{
		throw new NotImplementedException();
	}

	public bool Contains(string itemId, int amount)
	{
		throw new NotImplementedException();
	}

	public bool Contains(ItemStack item, int amount)
	{
		throw new NotImplementedException();
	}

	public bool ContainsAtLeast(ItemStack item, int amount)
	{
		throw new NotImplementedException();
	}

	public Dictionary<int, ItemStack> All(string itemId)
	{
		throw new NotImplementedException();
	}

	public Dictionary<int, ItemStack> All(ItemStack item)
	{
		throw new NotImplementedException();
	}

	public int First(string itemId)
	{
		// return index of first item matching id else -1
		return Array.FindIndex(_items, item => item?.Id == itemId);
	}

	public int First(ItemStack item)
	{
		// return index of first item matching id else -1
		return Array.FindIndex(_items, it => it.Id == item.Id && it.Amount == item.Amount);
	}

	public int FirstEmpty()
	{
		throw new NotImplementedException();
	}

	public void Remove(string itemId)
	{
		throw new NotImplementedException();
	}

	public void Remove(ItemStack item)
	{
		throw new NotImplementedException();
	}

	public void Clear(int index)
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}

	public string GetTitle()
	{
		throw new NotImplementedException();
	}

	public Dictionary<int, ItemStack> GetItemsOfCategory(ItemCategory category)
	{
		throw new NotImplementedException();
	}

	public IEnumerator<ItemStack> GetEnumerator()
	{
		return ((IEnumerable<ItemStack>) _items).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
