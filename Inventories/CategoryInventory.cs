using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Inventory;

namespace untitledplantgame.Inventory;

public class CategoryInventory : Inventory
{
	private readonly ItemCategory _category;

	public CategoryInventory(int size, string name, ItemCategory category)
		: base(size, name)
	{
		_category = category;
	}

	public bool CanStore(ItemStack item)
	{
		return item.Category.Equals(_category);
	}

	public override Dictionary<int, ItemStack> AddItem(params ItemStack[] items)
	{
		Dictionary<int, ItemStack> invalids = new();
		for (var i = 0; i < items.Length; i++)
		{
			if (!CanStore(items[i]))
			{
				invalids.Add(i, items[i]);
				items[i] = null;
			}
		}

		var overflow = base.AddItem(items);

		// combine overflow and invalids
		return invalids.Concat(overflow).GroupBy(kv => kv.Key).ToDictionary(g => g.Key, g => g.First().Value);
	}

	// TODO override other add/set
}
