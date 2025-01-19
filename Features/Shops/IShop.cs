using System;
using System.Collections.Generic;
using untitledplantgame.Inventory;

namespace untitledplantgame.Shops;

public interface IShop
{
	event Action<List<IItemStack>> ShopStockChanged;

	void SetShopContent(IItemStack[] items);

	void GenerateRandomShopStock(); // Might not be needed
	IItemStack[] CurrentStock { get; }
	IInventory Inventory { get; }

	/// <summary>
	/// Remove one item of that kind off the shop and returns that instance
	/// </summary>
	/// <param name="stack"></param>
	IItemStack BuyItem(IItemStack item);

	/// <summary>
	/// Remove one item of that kind off the shop and returns that instance
	/// </summary>
	/// <param name="slotIndex"></param>
	/// <returns></returns>
	IItemStack BuyItem(int slotIndex);
}
