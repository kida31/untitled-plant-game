using System;
using System.Collections.Generic;
using System.Diagnostics;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Shops;

public class SeedShop : IShop
{
	public event Action<List<ItemStack>> ShopStockChanged;

	public ItemStack[] CurrentStock => _shopInventory.GetContents().ToArray();

	private Inventory.Inventory _shopInventory;
	private Logger _logger = new Logger("SeedShopShop");

	public SeedShop()
	{
		_shopInventory = new Inventory.Inventory(16, "Seedshop");
	}

	public void SetShopContent(ItemStack[] items)
	{
		_shopInventory.SetContents(new List<ItemStack>(items));
		ShopStockChanged?.Invoke(_shopInventory.GetContents());
		_logger.Debug("[Set] ShopStockChanged");
	}

	public ItemStack BuyItem(ItemStack item)
	{
		Assert.AssertTrue(_shopInventory.Contains(item), "items did not exist");
		_shopInventory.RemoveItem(item);
		ShopStockChanged?.Invoke(_shopInventory.GetContents());
		_logger.Debug("[Buy] ShopStockChanged");
		return item;
	}

	public ItemStack BuyItem(int slotIndex)
	{
		var item = _shopInventory.GetItem(slotIndex).Clone() as ItemStack;
		item!.Amount = 1;
		ShopStockChanged?.Invoke(_shopInventory.GetContents());
		_logger.Debug("[Buy] ShopStockChanged");
		return BuyItem(item);
	}

	public void GenerateRandomShopStock()
	{
		var items = new RandomStockGenerator().GetRandom(15);
		SetShopContent(items.ToArray());
	}
}
