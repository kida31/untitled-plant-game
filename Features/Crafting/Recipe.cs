using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class Recipe : Resource
{
	private ItemStack _item;
	
	private ItemStack[] _ingredients;
}
