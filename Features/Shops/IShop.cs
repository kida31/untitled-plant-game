using System;
using System.Collections.Generic;
using untitledplantgame.Inventory;

namespace untitledplantgame.Shops;

public interface IShop
{
	event Action<List<ItemStack>> ShopStockChanged;

	void SetShopContent(ItemStack[] items);

	void GenerateRandomShopStock(); // Might not be needed
	ItemStack[] CurrentStock { get; }

	/// <summary>
	/// Remove one item of that kind off the shop and returns that instance
	/// </summary>
	/// <param name="stack"></param>
	ItemStack BuyItem(ItemStack item);

	/// <summary>
	/// Remove one item of that kind off the shop and returns that instance
	/// </summary>
	/// <param name="slotIndex"></param>
	/// <returns></returns>
	ItemStack BuyItem(int slotIndex);
}
