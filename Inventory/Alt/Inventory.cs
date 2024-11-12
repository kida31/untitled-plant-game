using System.Collections;
using System.Collections.Generic;

namespace untitledplantgame.Inventory.Alt;

public class Inventory: IInventory
{
	public int Size { get; }
	public int MaxStackSize { get; set; }
	public string Name { get; }
	
	private List<ItemStack> _items = new();
	
	public Inventory(int size, int maxStackSize, string name)
	{
		Size = size;
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

	public Dictionary<int, ItemStack> AddItem(params ItemStack[] items)
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
		item = item.Clone();
		while (item.Amount > 0)
		{
			var idx = GetFirstNonFull(item.Id);
			if (idx != -1)
			{
				var destination = _items[idx];
				var transferableAmount = MaxStackSize - destination.Amount;
				if (transferableAmount > item.Amount)
				{
					destination.Amount += item.Amount;
					item.Amount = 0;
				}
				else
				{
					destination.Amount = MaxStackSize;
					item.Amount -= transferableAmount;
				}

				continue;
			}

			idx = FirstEmpty();
			if (idx != -1)
			{
				_items[idx] = item;
				return null;
			}

			return item;
		}
	}

	private int GetFirstNonFull(string itemId)
	{
		for (var i = 0; i < _items.Count; i++)
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
		throw new System.NotImplementedException();
	}

	public List<ItemStack> GetContents()
	{
		throw new System.NotImplementedException();
	}

	public void SetContents(List<ItemStack> items)
	{
		throw new System.NotImplementedException();
	}

	public bool Contains(string itemId)
	{
		throw new System.NotImplementedException();
	}

	public bool Contains(ItemStack item)
	{
		throw new System.NotImplementedException();
	}

	public bool Contains(string itemId, int amount)
	{
		throw new System.NotImplementedException();
	}

	public bool Contains(ItemStack item, int amount)
	{
		throw new System.NotImplementedException();
	}

	public bool ContainsAtLeast(ItemStack item, int amount)
	{
		throw new System.NotImplementedException();
	}

	public Dictionary<int, ItemStack> All(string itemId)
	{
		throw new System.NotImplementedException();
	}

	public Dictionary<int, ItemStack> All(ItemStack item)
	{
		throw new System.NotImplementedException();
	}

	public int First(string itemId)
	{
		throw new System.NotImplementedException();
	}

	public int First(ItemStack item)
	{
		throw new System.NotImplementedException();
	}

	public int FirstEmpty()
	{
		throw new System.NotImplementedException();
	}

	public void Remove(string itemId)
	{
		throw new System.NotImplementedException();
	}

	public void Remove(ItemStack item)
	{
		throw new System.NotImplementedException();
	}

	public void Clear(int index)
	{
		throw new System.NotImplementedException();
	}

	public void Clear()
	{
		throw new System.NotImplementedException();
	}

	public string GetTitle()
	{
		throw new System.NotImplementedException();
	}

	public Dictionary<int, ItemStack> GetItemsOfCategory(ItemCategory category)
	{
		throw new System.NotImplementedException();
	}

	public IEnumerator<ItemStack> GetEnumerator()
	{
		throw new System.NotImplementedException();

	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
