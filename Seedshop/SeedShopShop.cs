using System;
using System.Collections.Generic;
using System.Diagnostics;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Seedshop;

public class SeedShopShop: ISeedShop
{
	public event Action<List<ItemStack>> ShopStockChanged;
	private Inventory.Inventory _shopInventory;
	private Logger _logger = new Logger("SeedShopShop");
	
	public SeedShopShop()
	{
		_shopInventory = new Inventory.Inventory(15, "Seedshop");
	}

	public void SetShopContent(ItemStack[] items)
	{
		_shopInventory.SetContents(new List<ItemStack>(items));
		ShopStockChanged?.Invoke(_shopInventory.GetContents());
		_logger.Debug("[Set] ShopStockChanged");
	}

	// TODO: Make this actually random according to some rules
	public ItemStack[] CurrentStock => _shopInventory.GetContents().ToArray();

	public ItemStack BuyItem(ItemStack item)
	{
		Debug.Assert(_shopInventory.Contains(item), "items did not exist");
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
		var items = new List<ItemStack>();
		
		{
			var item = new ItemStack("basil", "Basil", null, "This is basil", ItemCategory.Plant, 64, 10);
			item.Amount = 5;
			items.Add(item);
		}

		{
			var item = new ItemStack("parsley", "Parsley", null, "This is parsley", ItemCategory.Plant, 64, 15);
			item.Amount = 3;
			items.Add(item);
		}

		{
			var item = new ItemStack("mint", "Mint", null, "This is mint", ItemCategory.Plant, 64, 20);
			item.Amount = 2;
			items.Add(item);
		}

		{
			var item = new ItemStack("cilantro", "Cilantro", null, "This is cilantro", ItemCategory.Plant, 64, 12);
			item.Amount = 8;
			items.Add(item);
		}

		{
			var item = new ItemStack("oregano", "Oregano", null, "This is oregano", ItemCategory.Plant, 64, 14);
			item.Amount = 6;
			items.Add(item);
		}

		{
			var item = new ItemStack("thyme", "Thyme", null, "This is thyme", ItemCategory.Plant, 64, 18);
			item.Amount = 4;
			items.Add(item);
		}

		{
			var item = new ItemStack("rosemary", "Rosemary", null, "This is rosemary", ItemCategory.Plant, 64, 25);
			item.Amount = 2;
			items.Add(item);
		}

		{
			var item = new ItemStack("sage", "Sage", null, "This is sage", ItemCategory.Plant, 64, 17);
			item.Amount = 7;
			items.Add(item);
		}

		{
			var item = new ItemStack("chives", "Chives", null, "This is chives", ItemCategory.Plant, 64, 13);
			item.Amount = 10;
			items.Add(item);
		}

		{
			var item = new ItemStack("dill", "Dill", null, "This is dill", ItemCategory.Plant, 64, 11);
			item.Amount = 9;
			items.Add(item);
		}
		{
			var item = new ItemStack("lavender", "Lavender", null, "This is lavender", ItemCategory.Plant, 64, 30);
			item.Amount = 1;
			items.Add(item);
		}

		{
			var item = new ItemStack("tarragon", "Tarragon", null, "This is tarragon", ItemCategory.Plant, 64, 22);
			item.Amount = 4;
			items.Add(item);
		}

		{
			var item = new ItemStack("fennel", "Fennel", null, "This is fennel", ItemCategory.Plant, 64, 16);
			item.Amount = 6;
			items.Add(item);
		}

		{
			var item = new ItemStack("marjoram", "Marjoram", null, "This is marjoram", ItemCategory.Plant, 64, 19);
			item.Amount = 5;
			items.Add(item);
		}

		{
			var item = new ItemStack("lemonbalm", "Lemon Balm", null, "This is lemon balm", ItemCategory.Plant, 64, 14);
			item.Amount = 8;
			items.Add(item);
		}

		{
			var item = new ItemStack("chervil", "Chervil", null, "This is chervil", ItemCategory.Plant, 64, 12);
			item.Amount = 10;
			items.Add(item);
		}
		
		SetShopContent(items.ToArray());
	}
}
