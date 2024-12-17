using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Shops;

public class RandomStockGenerator
{
	private static Random rand = new(7);

	[Obsolete]
	public List<ItemStack> GetRandom(int n)
	{
		Assert.AssertTrue(n > 0);
		var items = new List<ItemStack>()
		{
			new("basil", "Basil", null, "This is basil", ItemCategory.Plant, baseValue: 10, amount: 5),
			new("parsley", "Parsley", null, "This is parsley", ItemCategory.Plant, baseValue: 15, amount: 3),
			new("mint", "Mint", null, "This is mint", ItemCategory.Plant, baseValue: 20, amount: 2),
			new("cilantro", "Cilantro", null, "This is cilantro", ItemCategory.Plant, baseValue: 12, amount: 8),
			new("oregano", "Oregano", null, "This is oregano", ItemCategory.Plant, baseValue: 14, amount: 6),
			new("thyme", "Thyme", null, "This is thyme", ItemCategory.Plant, baseValue: 18, amount: 4),
			new("rosemary", "Rosemary", null, "This is rosemary", ItemCategory.Plant, baseValue: 25, amount: 2),
			new("sage", "Sage", null, "This is sage", ItemCategory.Plant, baseValue: 17, amount: 7),
			new("chives", "Chives", null, "This is chives", ItemCategory.Plant, baseValue: 13, amount: 10),
			new("dill", "Dill", null, "This is dill", ItemCategory.Plant, baseValue: 11, amount: 9),
			new("lavender", "Lavender", null, "This is lavender", ItemCategory.Plant, baseValue: 30, amount: 1),
			new("tarragon", "Tarragon", null, "This is tarragon", ItemCategory.Plant, baseValue: 22, amount: 4),
			new("fennel", "Fennel", null, "This is fennel", ItemCategory.Plant, baseValue: 16, amount: 6),
			new("marjoram", "Marjoram", null, "This is marjoram", ItemCategory.Plant, baseValue: 19, amount: 5),
			new("lemonbalm", "Lemon Balm", null, "This is lemon balm", ItemCategory.Plant, baseValue: 14, amount: 8),
			new("chervil", "Chervil", null, "This is chervil", ItemCategory.Plant, baseValue: 12, amount: 10),
		}
			.OrderBy(o => rand.Next())
			.ToList();
		return items.GetRange(0, Math.Min(items.Count, n));
	}
}
