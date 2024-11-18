using untitledplantgame.Inventory;

namespace untitledplantgame.Seedshop;

public interface ISeedshop
{
	public ItemStack[] SetShopContent();
	
	public void GenerateRandomShopStock();
	public ItemStack[] CurrentStock { get; }

	/// <summary>
	/// Remove one item of that kind off the shop and returns that instance
	/// </summary>
	/// <param name="stack"></param>
	public ItemStack BuyItem(ItemStack item);

	/// <summary>
	/// Remove one item of that kind off the shop and returns that instance
	/// </summary>
	/// <param name="slotIndex"></param>
	/// <returns></returns>
	public ItemStack BuyItem(int slotIndex);
}
