using System;
using System.Collections.Generic;
using untitledplantgame.Inventory;

namespace untitledplantgame.Shops;

public class RandomStockGenerator
{
	// TODO: Not actually random, just a fixed list. Make this actually random according to some rules
	[Obsolete]
	public List<ItemStack> GetRandom(int n)
	{
		return new List<ItemStack>()
		{
			new("basil", "Basil", null, "This is basil", ItemCategory.Medicine, 64, 10, 5),
			new("parsley", "Parsley", null, "This is parsley", ItemCategory.Medicine, 64, 15, 3),
			new("mint", "Mint", null, "This is mint", ItemCategory.Medicine, 64, 20, 2),
			new("cilantro", "Cilantro", null, "This is cilantro", ItemCategory.Medicine, 64, 12, 8),
			new("oregano", "Oregano", null, "This is oregano", ItemCategory.Medicine, 64, 14, 6),
			new("thyme", "Thyme", null, "This is thyme", ItemCategory.Medicine, 64, 18, 4),
			new("rosemary", "Rosemary", null, "This is rosemary", ItemCategory.Medicine, 64, 25, 2),
			new("sage", "Sage", null, "This is sage", ItemCategory.Medicine, 64, 17, 7),
			new("chives", "Chives", null, "This is chives", ItemCategory.Medicine, 64, 13, 10),
			new("dill", "Dill", null, "This is dill", ItemCategory.Medicine, 64, 11, 9),
			new("lavender", "Lavender", null, "This is lavender", ItemCategory.Medicine, 64, 30, 1),
			new("tarragon", "Tarragon", null, "This is tarragon", ItemCategory.Medicine, 64, 22, 4),
			new("fennel", "Fennel", null, "This is fennel", ItemCategory.Medicine, 64, 16, 6),
			new("marjoram", "Marjoram", null, "This is marjoram", ItemCategory.Medicine, 64, 19, 5),
			new("lemonbalm", "Lemon Balm", null, "This is lemon balm", ItemCategory.Medicine, 64, 14, 8),
			new("chervil", "Chervil", null, "This is chervil", ItemCategory.Medicine, 64, 12, 10),
		};
	}
}
