using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Shops;

public class SeedShop : IShop
{
	public event Action<List<IItemStack>> ShopStockChanged;

	public IItemStack[] CurrentStock => Inventory.GetItems().ToArray();

	public IInventory Inventory { get; private set; }
	private Logger _logger = new Logger("SeedShopShop");

	public SeedShop()
	{
		Inventory = new Inventory.Inventory(12, "Seedshop");
		GenerateRandomShopStock();
		GD.PrintRaw(BbImage.Coin);
	}

	public void SetShopContent(IItemStack[] items)
	{
		Inventory.SetContents(new List<IItemStack>(items));
		ShopStockChanged?.Invoke(Inventory.GetItems());
		_logger.Debug("[Set] ShopStockChanged");
	}

	public IItemStack BuyItem(IItemStack item)
	{
		Assert.AssertTrue(Inventory.Contains(item), "items did not exist");
		Inventory.RemoveItem(item);
		ShopStockChanged?.Invoke(Inventory.GetItems());
		_logger.Debug("[Buy] ShopStockChanged");
		return item;
	}

	public IItemStack BuyItem(int slotIndex)
	{
		var item = Inventory.GetItem(slotIndex).Clone() as IItemStack;
		item!.Amount = 1;
		ShopStockChanged?.Invoke(Inventory.GetItems());
		_logger.Debug("[Buy] ShopStockChanged");
		return BuyItem(item);
	}

	public void GenerateRandomShopStock()
	{
		var items = new RandomStockGenerator().GetRandomItems(12);
		SetShopContent(items.ToArray());
	}
}
