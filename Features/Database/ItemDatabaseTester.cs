using System.Collections.Generic;
using Godot;
using untitledplantgame.Crafting;
using untitledplantgame.Entity;
using untitledplantgame.Inventory;
using untitledplantgame.Item.Components;
using untitledplantgame.Statistics;
using untitledplantgame.Statistics.StatTypes;

namespace untitledplantgame.Database;

public partial class ItemDatabaseTester : Node
{
	public override void _Ready()
	{
		/*
		// ItemDatabase.Instance.MyAsyncFunction(1000, 1000);
		// GD.Print(ItemDatabase.Instance.GetItemStackById("tenthItem").Description);
		// var list = ItemDatabase.Instance.GetItemStacksWithSpecifiedComponents(new List<AComponent>{ new Leaf(), new Antioxidant(), new Mint() });
		ItemStack itemStack = ItemDatabase.Instance.GetItemStackById("Sunflower");
		ItemStack itemStack2 = ItemDatabase.Instance.GetItemStackById("MintLeaf");
		ItemStack itemStack3 = ItemDatabase.Instance.GetItemStackById("BasilLeaf");

		List<Recipe> recipes1 =
			ItemDatabase.Instance.GetAllRecipesWithItemStacksAndCraftingType(new List<ItemStack> { itemStack3 }, null,
				Recipe.CraftingType.Unspecified);

		foreach (var recipe in recipes1)
		{
			// This still gives us the NUKE recipe (but this has to be the case)
			// We have: Sunflower x4, MintLeaf x1 => Every Recipe with AT LEAST one Sunflower and MintLeaf 
			GD.Print(recipe.RecipeCraftingType);
		}
		
		var recipe5 = new Recipe(
			Recipe.CraftingType.Unspecified,
			new List<IRecipeFilterPart>
			{
				new ComponentList
				{
					new Basil()
				}
			},
			ItemDatabase.Instance.GetItemStackById("GameEndingNuke")
		);
		
		GD.Print(recipe5.CraftResult(new List<ItemStack>{ItemDatabase.Instance.GetItemStackById("BasilLeaf")}).Description);

		
		var e1 = new EntityConfiguration();
		e1.Stats.Add(new Stat(10, new Currency(), false));
		e1.Stats.Add(new Stat(10, new Faith(), false));
		e1.Stats.Add(new Stat(10, new MovementSpeed(), false));
		
		var e2 = new EntityConfiguration();
		e2.Stats.Add(new Stat(1, new Currency(), false));
		e2.Stats.Add(new Stat(1, new Health(), false));

		e1.CombineComponent(e2);

		foreach (var stat in e1.Stats)
		{
			GD.Print(stat.GetModifiedStatValue());
		}


		var testingRecipe = new Recipe(
			Recipe.CraftingType.Cooking,
			new List<IRecipeFilterPart>
			{
				new ComponentList
				{
					new Basil()
				},
				new ItemId("MintLeaf"),
			},
			null,
			null
		);

		var craftResult = testingRecipe.CraftResult(
			new List<ItemStack>{ 
				ItemDatabase.Instance.GetItemStackById("BasilLeaf"), 
				ItemDatabase.Instance.GetItemStackById("MintLeaf")}
			);
		
		GD.Print(craftResult.Id);
		*/
		
		
		
		
		//--------------------------------------------------------------------------------------------------------------------------------//
		
		
		// 1. Potential multithreaded solution:
		//ItemDatabase.Instance.MyAsyncFunction(1000, 1000);
		
		
		// 2. Access the Database for a single ItemStack by ID and Access it
		ItemStack dummyItemStack = ItemDatabase.Instance.GetItemStackById("BasilLeaf");
		GD.Print(dummyItemStack.Id);
		GD.Print(dummyItemStack.Name);
		GD.Print(dummyItemStack.Description);
		GD.Print(dummyItemStack.Amount);
		GD.Print(dummyItemStack.Category);
		
		
		// 3. Get a Recipe based on ItemStack(s) => returns all Recipes containing AT LEAST 
		List<Recipe> dummyRecipes =
			ItemDatabase.Instance.GetAllRecipesWithItemStacksAndCraftingType(
				new List<ItemStack> { dummyItemStack }, 
				null,
				Recipe.CraftingType.Cooking);

		foreach (var recipe in dummyRecipes)
		{
			GD.Print(recipe.RecipeCraftingType);
		}
		
		
		// 4. Creating a custom Recipe to show how all of it works
		var customRecipe = new Recipe(
			Recipe.CraftingType.Unspecified,
			new List<IRecipeFilterPart>
			{
				new ComponentList
				{
					new Basil(),
				},
				new ComponentList
				{
					new Basil(),
				}
			},
			ItemDatabase.Instance.GetItemStackById("GameEndingNuke")
		);

		var temp= ItemDatabase.Instance.GetItemStackById("BasilLeaf");
		GD.Print(customRecipe.CraftResult(new List<ItemStack>{temp}));
	}
}
