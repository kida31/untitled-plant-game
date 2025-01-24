using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Database;
using untitledplantgame.Inventory;
using untitledplantgame.Item;
using untitledplantgame.Statistics.StatTypes;
using untitledplantgame.Systems;

namespace untitledplantgame.Shops;

public class SeedShop : IShop
{
	private static readonly Random Random = new(42);
	private const int MaxItemVariety = 12;
	private const int MinItemVariety = 6;
	private const int MaxItemAmount = 1;
	private const int MinItemAmount = 1;

	public event Action<List<IItemStack>> ShopStockChanged;

	public IItemStack[] CurrentStock => Inventory.GetItems().ToArray();

	public IInventory Inventory { get; private set; }
	private Logger _logger = new Logger("SeedShopShop");

	public SeedShop()
	{
		Inventory = new Inventory.Inventory(12, "Seedshop");
	}

	public void SetShopContent(IItemStack[] items)
	{
		Inventory.SetContents(new List<IItemStack>(items));
		ShopStockChanged?.Invoke(Inventory.GetItems());
		_logger.Debug("[Set] ShopStockChanged");
	}

	public IItemStack BuyItem(IItemStack item)
	{
		Assert.AssertTrue(item.BaseValue <= CurrencyFaithOfficer.Instance.GetCurrentCurrency(),
			"Not enough money to purchase. Triggering buy should not have been possible");
		Assert.AssertTrue(Inventory.Contains(item), "items did not exist");

		// Remove item from shop
		Inventory.RemoveItem(item);
		ShopStockChanged?.Invoke(Inventory.GetItems());
		_logger.Debug("[Buy] ShopStockChanged");

		// Deduce Money
		CurrencyFaithOfficer.Instance.ChangeAny(new Currency(), -item.BaseValue);

		// Add item to player inventory
		Game.Player.Inventory.AddItem(item);
		return item;
	}

	public IItemStack BuyItem(int slotIndex)
	{
		var item = Inventory.GetItem(slotIndex).Clone();
		item!.Amount = 1;
		ShopStockChanged?.Invoke(Inventory.GetItems());
		_logger.Debug("[Buy] ShopStockChanged");
		return BuyItem(item);
	}

	public void GenerateRandomShopStock()
	{
		// Placeholder
		var allSeeds = ItemDatabase.Instance.ItemStacks
			.Where(it => it.Category == ItemCategory.Seed);
		var seedsWithRandomAmount = allSeeds
			.Select(s =>
			{
				var seed = s.Clone();
				seed.Amount = Random.Next(MinItemAmount, MaxItemAmount + 1);
				return seed;
			})
			.ToList();

		var count = Math.Min(Random.Next(MinItemVariety, MaxItemVariety + 1), seedsWithRandomAmount.Count);
		var randomSeeds = seedsWithRandomAmount
			.OrderBy(_ => Random.Next())
			.Take(count);

		SetShopContent(randomSeeds.ToArray());
	}
}
