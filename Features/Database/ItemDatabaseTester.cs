using System.Collections.Generic;
using Godot;
using untitledplantgame.Crafting;
using untitledplantgame.Inventory;
namespace untitledplantgame.Database;

public partial class ItemDatabaseTester : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// ItemDatabase.Instance.MyAsyncFunction(1000, 1000);
		// GD.Print(ItemDatabase.Instance.GetItemStackById("tenthItem").Description);
		// var list = ItemDatabase.Instance.GetItemStacksWithSpecifiedComponents(new List<AComponent>{ new Leaf(), new Antioxidant(), new Mint() });
		ItemStack itemStack = ItemDatabase.Instance.GetItemStackById("Sunflower");
		ItemStack itemStack2 = ItemDatabase.Instance.GetItemStackById("MintLeaf");
		
		List<Recipe> recipes = ItemDatabase.Instance.GetAllRecipesWithItemStacks(new List<ItemStack> { itemStack });

		foreach (var recipe in recipes)
		{
			GD.Print(recipe.RecipeCraftingType);
		}
	}

	// Called every frame. 'Delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
