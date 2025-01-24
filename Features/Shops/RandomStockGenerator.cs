using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Item;
using untitledplantgame.Item.Components;

namespace untitledplantgame.Shops;

/// <summary>
///     Generates random stock for shops. And provides misc. random item generation.
/// </summary>
public class RandomStockGenerator
{
	private static readonly Random Rand = new(34);

	/// <summary>
	///     Generates random placeholder items. These do not exist in the game.
	/// </summary>
	/// <param name="n"></param>
	/// <returns></returns>
	[Obsolete]
	public List<IItemStack> GetRandomPlaceholders(int n)
	{
		Assert.AssertTrue(n > 0);
		var items = new List<IItemStack>()
			{
				new ItemStack("basil")
				{
					Name = "Basil",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is basil",
					BaseValue = 10,
					Amount = 5
				},
				new ItemStack("parsley")
				{
					Name = "Parsley",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is parsley",
					BaseValue = 15,
					Amount = 3
				},
				new ItemStack("mint")
				{
					Name = "Mint",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is mint",
					BaseValue = 20,
					Amount = 2
				},
				new ItemStack("cilantro")
				{
					Name = "Cilantro",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is cilantro",
					BaseValue = 12,
					Amount = 8
				},
				new ItemStack("oregano")
				{
					Name = "Oregano",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is oregano",
					BaseValue = 14,
					Amount = 6
				},
				new ItemStack("thyme")
				{
					Name = "Thyme",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is thyme",
					BaseValue = 18,
					Amount = 4
				},
				new ItemStack("rosemary")
				{
					Name = "Rosemary",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is rosemary",
					BaseValue = 25,
					Amount = 2
				},
				new ItemStack("sage")
				{
					Name = "Sage",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is sage",
					BaseValue = 17,
					Amount = 7
				},
				new ItemStack("chives")
				{
					Name = "Chives",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is chives",
					BaseValue = 13,
					Amount = 10
				},
				new ItemStack("dill")
				{
					Name = "Dill",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is dill",
					BaseValue = 11,
					Amount = 9
				},
				new ItemStack("lavender")
				{
					Name = "Lavender",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is lavender",
					BaseValue = 30,
					Amount = 1
				},
				new ItemStack("tarragon")
				{
					Name = "Tarragon",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is tarragon",
					BaseValue = 22,
					Amount = 4
				},
				new ItemStack("fennel")
				{
					Name = "Fennel",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is fennel",
					BaseValue = 16,
					Amount = 6
				},
				new ItemStack("marjoram")
				{
					Name = "Marjoram",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is marjoram",
					BaseValue = 19,
					Amount = 5
				},
				new ItemStack("lemonbalm")
				{
					Name = "Lemon Balm",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is lemon balm",
					BaseValue = 14,
					Amount = 8
				},
				new ItemStack("chervil")
				{
					Name = "Chervil",
					Category = ItemCategory.Seed,
					ToolTipDescription = "This is chervil",
					BaseValue = 12,
					Amount = 10
				},
			}
			.OrderBy(o => Rand.Next())
			.ToList();
		return items.GetRange(0, Math.Min(items.Count, n));
	}

	/// <summary>
	///     Generates random items from the item database.
	/// </summary>
	/// <param name="n"></param>
	/// <returns></returns>
	public List<IItemStack> GetRandomItems(int n)
	{
		var items = ItemDatabase.Instance.GetAllItems()
			// Ignore dried objects
			.Where(o => !o.GetComponent<TagsComponent>()?.Contains(TagsComponent.Tags.IsDried) ?? true)
			.OrderBy(o => Rand.Next())
			.ToList<IItemStack>();
		items.ForEach(it => it.Amount = Rand.Next(1, it.MaxStackSize));
		return items.GetRange(0, Math.Min(items.Count, n));
	}
}
