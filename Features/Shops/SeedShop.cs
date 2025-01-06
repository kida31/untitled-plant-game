using System;
using System.Collections.Generic;
using System.Diagnostics;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Shops;

public class SeedShop : IShop
{
	public event Action<List<IItemStack>> ShopStockChanged;

	public IItemStack[] CurrentStock => _shopInventory.GetItems().ToArray();

	private Inventory.Inventory _shopInventory;
	private Logger _logger = new Logger("SeedShopShop");

	public SeedShop()
	{
		_shopInventory = new Inventory.Inventory(16, "Seedshop");
	}

	public void SetShopContent(IItemStack[] items)
	{
		_shopInventory.SetContents(new List<IItemStack>(items));
		ShopStockChanged?.Invoke(_shopInventory.GetItems());
		_logger.Debug("[Set] ShopStockChanged");
	}

	public IItemStack BuyItem(IItemStack item)
	{
		Assert.AssertTrue(_shopInventory.Contains(item), "items did not exist");
		_shopInventory.RemoveItem(item);
		ShopStockChanged?.Invoke(_shopInventory.GetItems());
		_logger.Debug("[Buy] ShopStockChanged");
		return item;
	}

	public IItemStack BuyItem(int slotIndex)
	{
		var item = _shopInventory.GetItem(slotIndex).Clone() as ItemStack;
		item!.Amount = 1;
		ShopStockChanged?.Invoke(_shopInventory.GetItems());
		_logger.Debug("[Buy] ShopStockChanged");
		return BuyItem(item);
	}

	public void GenerateRandomShopStock()
	{
		var items = new RandomStockGenerator().GetRandomPlaceholders(15);
		SetShopContent(items.ToArray());
	}
}
