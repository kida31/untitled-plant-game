using System;
using System.Collections.Generic;
using untitledplantgame.Inventory;

namespace untitledplantgame.Shops;

public class RandomStockGenerator
{
	// TODO: Not actually random, just a fixed list. Make this actually random according to some rules
	[Obsolete]
	public List<IItemStack> GetRandom(int n)
	{
		return new List<IItemStack>()
		{
			new ItemStack("basil", "Basil", null, "This is basil", "", ItemCategory.Seed, baseValue: 10, amount: 5),
			new ItemStack("parsley", "Parsley", null, "This is parsley", "",ItemCategory.Seed, baseValue: 15, amount: 3),
			new ItemStack("mint", "Mint", null, "This is mint", "",ItemCategory.Seed, baseValue: 20, amount: 2),
			new ItemStack("cilantro", "Cilantro", null, "This is cilantro","", ItemCategory.Seed, baseValue: 12, amount: 8),
			new ItemStack("oregano", "Oregano", null, "This is oregano", "",ItemCategory.Seed, baseValue: 14, amount: 6),
			new ItemStack("thyme", "Thyme", null, "This is thyme","", ItemCategory.Seed, baseValue: 18, amount: 4),
			new ItemStack("rosemary", "Rosemary", null, "This is rosemary","", ItemCategory.Seed, baseValue: 25, amount: 2),
			new ItemStack("sage", "Sage", null, "This is sage","", ItemCategory.Seed, baseValue: 17, amount: 7),
			new ItemStack("chives", "Chives", null, "This is chives", "",ItemCategory.Seed, baseValue: 13, amount: 10),
			new ItemStack("dill", "Dill", null, "This is dill","", ItemCategory.Seed, baseValue: 11, amount: 9),
			new ItemStack("lavender", "Lavender", null, "This is lavender","", ItemCategory.Seed, baseValue: 30, amount: 1),
			new ItemStack("tarragon", "Tarragon", null, "This is tarragon", "",ItemCategory.Seed, baseValue: 22, amount: 4),
			new ItemStack("fennel", "Fennel", null, "This is fennel","", ItemCategory.Seed, baseValue: 16, amount: 6),
			new ItemStack("marjoram", "Marjoram", null, "This is marjoram","", ItemCategory.Seed, baseValue: 19, amount: 5),
			new ItemStack("lemonbalm", "Lemon Balm", null, "This is lemon balm", "",ItemCategory.Seed, baseValue: 14, amount: 8),
			new ItemStack("chervil", "Chervil", null, "This is chervil", "",ItemCategory.Seed, baseValue: 12, amount: 10),
		};
	}
}
