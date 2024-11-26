using Godot;
using untitledplantgame.Crafting;
using untitledplantgame.Inventory;

namespace untitledplantgame.Item;

public interface IItemDatabase
{
	/// <summary>
	/// Returns a new item based on ingredient and the method used to craft it.
	/// </summary>
	/// <param name="baseItem">base ingredient</param>
	/// <param name="method">crafting method</param>
	/// <returns></returns>
	Resource RequestCraftedItem(ItemStack baseItem, CraftMethod method);
	
	/// <summary>
	/// Returns a new item by combining the ingredients.
	/// item ids are sorted alphabetically and concatenated.
	/// </summary>
	/// <param name="ingredients">ingredients</param>
	/// <returns></returns>
	Resource RequestMixedItem(ItemStack[] ingredients);
	
	/// <summary>
	/// Get item by id.
	/// </summary>
	/// <param name="id"></param>
	/// <returns>Item</returns>
	Resource GetItem(string id);
	
	/// <summary>
	/// Get all items.
	/// </summary>
	/// <returns>all Items</returns>
	Resource[] GetAllItems();
}
