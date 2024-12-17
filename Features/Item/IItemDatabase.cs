using untitledplantgame.Inventory;

namespace untitledplantgame.Item;

public interface IItemDatabase
{
	/// <summary>
	/// Returns a new item by combining the ingredients.
	/// item ids are sorted alphabetically and concatenated.
	/// </summary>
	/// <param name="ingredients">ingredients</param>
	/// <returns></returns>
	ItemStack RequestMixedItem(ItemStack[] ingredients);

	/// <summary>
	/// Get item by id.
	/// </summary>
	/// <param name="id"></param>
	/// <returns>Item</returns>
	ItemStack GetItem(string id);

	/// <summary>
	/// Get all items.
	/// </summary>
	/// <returns>all Items</returns>
	ItemStack[] GetAllItems();
}
