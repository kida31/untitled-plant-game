using System.Collections.Generic;
using System.Linq;

namespace untitledplantgame.Inventory.Alt;

public class RestrictedInventory : Inventory
{
	private ItemCategory _category;

	public RestrictedInventory(int size, int maxStackSize, string name, ItemCategory category) : base(size, maxStackSize, name)
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
		return invalids.Concat(overflow)
			.GroupBy(kv => kv.Key)
			.ToDictionary(g => g.Key, g => g.First().Value);
	}

	// TODO override other add/set
}
