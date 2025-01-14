using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory;

/// <summary>
///     A big inventory that contains multiple sub-inventories.
///     Inventories have contiguous indexes.
///     <example>
///          <list type="bullet">
///               <item>inventory A: [0, 1, 2, 3, 4]</item>
///               <item>inventory B: [5, 6, 7, 8, 9]</item>
///               <item>inventory C: [10, 11, 12, 13, 14]</item>
///          </list>
///     </example>
///     <remarks>
///         Check <see cref="IInventory">the interface</see> for functionality details
///     </remarks>
/// </summary>
public class BigInventory : IInventory
{
	public event Action InventoryChanged;
	public event Action<IItemStack> ItemAdded;
	public event Action<IItemStack> ItemRemoved;
	
	public int Size => _inventories.Values.Sum(inventory => inventory.Size);
	public string Name => "Player Inventory";
	
	private readonly Dictionary<ItemCategory, IInventory> _inventories;
	private readonly Logger _logger = new("BigInventory");
	
	public BigInventory(int size)
	{
		_inventories = new Dictionary<ItemCategory, IInventory>
		{
			{ ItemCategory.Seed, new Inventory(size, "Seeds") },
			{ ItemCategory.Material, new Inventory(size, "Materials") },
			{ ItemCategory.Medicine, new Inventory(size, "Medicine") },
		};
		
		foreach (var (_, inventory) in _inventories)
		{
			inventory.InventoryChanged += () => InventoryChanged?.Invoke();
			inventory.ItemAdded += item => ItemAdded?.Invoke(item);
			inventory.ItemRemoved += item => ItemRemoved?.Invoke(item);
		}
	}

	public BigInventory(Dictionary<ItemCategory, IInventory> inventories)
	{
		_inventories = inventories;
	}

	public List<IInventory> GetSubInventories()
	{
		return new List<IInventory>(_inventories.Values);
	}

	public IInventory GetInventory(ItemCategory category)
	{
		return _inventories[category];
	}

	public IEnumerator<IItemStack> GetEnumerator()
	{
		return _inventories.Values.SelectMany(inventory => inventory).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <summary>
	/// Return item of item from the big inventory.
	/// Assumes contiguous indexes throughout the inventories.
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public IItemStack GetItem(int index)
	{
		foreach (var inventory in _inventories.Values)
		{
			if (index < inventory.Size)
			{
				return inventory.GetItem(index);
			}

			index -= inventory.Size;
		}

		return null;
	}

	public void SetItem(int index, IItemStack item)
	{
		foreach (var inventory in _inventories.Values)
		{
			if (index < inventory.Size)
			{
				inventory.SetItem(index, item);
				return;
			}

			index -= inventory.Size;
		}
	}

	public Dictionary<int, IItemStack> AddItem(params IItemStack[] items)
	{
		var overflow = new Dictionary<int, IItemStack>();
		for (var index = 0; index < items.Length; index++)
		{
			var item = items[index];
			Assert.AssertNotNull(_inventories, "Inventories is null");
			Assert.AssertNotNull(item, "Item is null");
			Assert.AssertNotNull(item.Category, "Item category is null");
			var inventory = _inventories[item.Category];
			var leftover = inventory.AddItem(item);

			// Assertion, should never happen
			if (leftover == null || leftover.Count > 1)
			{
				_logger.Error("Single AddItem return more than one stack");
				return overflow;
			}

			// Append dictionary entry (1!) to overflow
			if (leftover.Count > 0)
			{
				overflow.Add(index, leftover[0]);
			}
		}

		return overflow;
	}

	public Dictionary<int, IItemStack> RemoveItem(params IItemStack[] items)
	{
		var remainders = new Dictionary<int, IItemStack>();
		for (var index = 0; index < items.Length; index++)
		{
			var item = items[index];
			var inventory = _inventories[item.Category];
			var remainingStack = inventory.RemoveItem(item);

			// Assertion, should never happen
			if (remainingStack == null || remainingStack.Count > 1)
			{
				_logger.Error("Single AddItem return more than one stack");
				return remainders;
			}

			// Append dictionary entry (1!) to overflow
			if (remainingStack.Count > 0)
			{
				remainders.Add(index, remainingStack[0]);
			}
		}

		return remainders;
	}

	public List<IItemStack> GetItems()
	{
		return _inventories.Values.SelectMany(inventory => inventory.GetItems()).ToList();
	}

	public void SetContents(List<IItemStack> items)
	{
		if (items.Count > Size)
		{
			_logger.Error("Too many items to fit in inventory");
			return;
		}

		var index = 0;
		foreach (var inventory in _inventories.Values)
		{
			var sourceItems = items.GetRange(index, inventory.Size);
			inventory.SetContents(sourceItems);
			index += inventory.Size;
		}
	}

	public bool Contains(string itemId)
	{
		return _inventories.Values.Any(inventory => inventory.Contains(itemId));
	}

	public bool Contains(IItemStack item)
	{
		return _inventories[item.Category].Contains(item);
	}

	public bool Contains(string itemId, int amount)
	{
		return _inventories.Values.Any(inventory => inventory.Contains(itemId, amount));
	}

	public bool Contains(IItemStack item, int amount)
	{
		return _inventories[item.Category].Contains(item, amount);
	}

	public Dictionary<int, IItemStack> All(string itemId)
	{
		var results = new Dictionary<int, IItemStack>();
		var index = 0;
		foreach (var inventory in _inventories.Values)
		{
			var items = inventory.All(itemId);
			foreach (var (key, value) in items)
			{
				results.Add(key + index, value);
			}

			index += inventory.Size;
		}

		return results;
	}

	public Dictionary<int, IItemStack> All(IItemStack item)
	{
		var indexOffset = 0;
		var results = new Dictionary<int, IItemStack>();
		foreach (var (category, inventory) in _inventories)
		{
			
			if (category != item.Category)
			{
				indexOffset += inventory.Size;
				continue; // Skip rest since wrongInventory.All() will return empty dictionary
			} 
			
			// Get local indices and add offset for "global" index
			var indices = inventory.All(item);
			foreach (var (stackIndex, stack) in indices)
			{
				results.Add(stackIndex + indexOffset, stack);
			}
		}

		return results;
	}

	public int First(string itemId)
	{
		var indexOffset = 0;
		foreach (var inventory in _inventories.Values)
		{
			var first = inventory.First(itemId);
			if (first != -1)
			{
				return first + indexOffset;
			}

			indexOffset += inventory.Size;
		}

		return -1;
	}

	public int First(IItemStack item)
	{
		var indexOffset = 0;
		foreach (var (category, inventory) in _inventories)
		{
			// Technically redundant since wrongInventory.First() will return -1
			if (category != item.Category)
			{
				indexOffset += inventory.Size;
				continue;
			}

			var first = inventory.First(item);
			if (first != -1)
			{
				return first + indexOffset;
			}
		}

		return -1;
	}

	public void RemoveAll(string itemId)
	{
		foreach (var inventory in _inventories.Values)
		{
			inventory.RemoveAll(itemId);
		}
	}

	public void RemoveAll(IItemStack item)
	{
		foreach (var inventory in _inventories.Values)
		{
			inventory.RemoveAll(item);
		}
	}

	public void Clear(int index)
	{
		foreach (var inventory in _inventories.Values)
		{
			if (index < inventory.Size)
			{
				inventory.Clear(index);
				return;
			}

			index -= inventory.Size;
		}
	}

	public void Clear()
	{
		foreach (var inventory in _inventories.Values)
		{
			inventory.Clear();
		}
	}

	public Dictionary<int, IItemStack> GetItemsOfCategory(ItemCategory category)
	{
		var results = new Dictionary<int, IItemStack>();
		var indexOffset = 0;
		var localResults = _inventories[category].GetItemsOfCategory(category);
		foreach (var (index, stack) in localResults)
		{
			results.Add(index + indexOffset, stack);
			indexOffset += _inventories[category].Size;
		}

		return results;
	}

	public void QuickStack(IInventory target)
	{
		foreach (var inventory in _inventories.Values)
		{
			inventory.QuickStack(target);
		}
	}

	public IItemStack AddItemToSlot(int slotIdx, IItemStack item)
	{
		foreach (var inventory in _inventories.Values)
		{
			if (slotIdx < inventory.Size)
			{
				return inventory.AddItemToSlot(slotIdx, item);
			}

			slotIdx -= inventory.Size;
		}

		return null;
	}
}
