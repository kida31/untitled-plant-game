using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Database;
using untitledplantgame.Inventory;

namespace untitledplantgame.Shops;

public class RandomStockGenerator
{
	private static readonly Random Rand = new(7);

	[Obsolete]
	public List<ItemStack> GetRandomPlaceholders(int n)
	{
		Assert.AssertTrue(n > 0);
		var items = new List<ItemStack>()
			{
				new("basil")
				{
					Name = "Basil",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is basil",
					BaseValue = 10,
					Amount = 5
				},
				new("parsley")
				{
					Name = "Parsley",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is parsley",
					BaseValue = 15,
					Amount = 3
				},
				new("mint")
				{
					Name = "Mint",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is mint",
					BaseValue = 20,
					Amount = 2
				},
				new("cilantro")
				{
					Name = "Cilantro",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is cilantro",
					BaseValue = 12,
					Amount = 8
				},
				new("oregano")
				{
					Name = "Oregano",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is oregano",
					BaseValue = 14,
					Amount = 6
				},
				new("thyme")
				{
					Name = "Thyme",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is thyme",
					BaseValue = 18,
					Amount = 4
				},
				new("rosemary")
				{
					Name = "Rosemary",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is rosemary",
					BaseValue = 25,
					Amount = 2
				},
				new("sage")
				{
					Name = "Sage",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is sage",
					BaseValue = 17,
					Amount = 7
				},
				new("chives")
				{
					Name = "Chives",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is chives",
					BaseValue = 13,
					Amount = 10
				},
				new("dill")
				{
					Name = "Dill",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is dill",
					BaseValue = 11,
					Amount = 9
				},
				new("lavender")
				{
					Name = "Lavender",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is lavender",
					BaseValue = 30,
					Amount = 1
				},
				new("tarragon")
				{
					Name = "Tarragon",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is tarragon",
					BaseValue = 22,
					Amount = 4
				},
				new("fennel")
				{
					Name = "Fennel",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is fennel",
					BaseValue = 16,
					Amount = 6
				},
				new("marjoram")
				{
					Name = "Marjoram",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is marjoram",
					BaseValue = 19,
					Amount = 5
				},
				new("lemonbalm")
				{
					Name = "Lemon Balm",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is lemon balm",
					BaseValue = 14,
					Amount = 8
				},
				new("chervil")
				{
					Name = "Chervil",
					Category = ItemCategory.Plant,
					ToolTipDescription = "This is chervil",
					BaseValue = 12,
					Amount = 10
				},
			}
			.OrderBy(o => Rand.Next())
			.ToList();
		return items.GetRange(0, Math.Min(items.Count, n));
	}

	public List<IItemStack> GetRandomItems(int n)
	{
		var items = ItemDatabase.Instance.ItemStacks
			.OrderBy(o => Rand.Next())
			.ToList<IItemStack>();
		return items.GetRange(0, Math.Min(items.Count, n));
	}
}
