using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Crafting;
using untitledplantgame.Entity;
using untitledplantgame.Inventory;
using untitledplantgame.Item;
using untitledplantgame.Item.Components;
using untitledplantgame.Statistics;
using untitledplantgame.Statistics.StatTypes;

namespace untitledplantgame.Database;

[Tool]
public partial class ItemDatabaseTester : Node
{
	[Export]
	private ItemStack[] _items
	{
		get => ItemDatabase.Instance?.ItemStacks.ToArray();
		set { }
	}

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
		

		//--------------------------------------------------------------------------------------------------------------------------------//

		// 1. Potential multithreaded solution:
		//ItemDatabase.Instance.MyAsyncFunction(1000, 1000);

		// 2. Access the Database for a single ItemStack by ID and Access it
		ItemStack dummyItemStack = ItemDatabase.Instance.CreateItemStack("BasilLeaf");
		GD.Print(dummyItemStack.Id);
		GD.Print(dummyItemStack.Name);
		GD.Print(dummyItemStack.ToolTipDescription);
		GD.Print(dummyItemStack.Amount);
		GD.Print(dummyItemStack.Category);

		// 3. Get a Recipe based on ItemStack(s) => returns all Recipes containing AT LEAST
		List<Recipe> dummyRecipes = ItemDatabase.Instance.GetAllRecipesWithItemStacksAndCraftingType(
			new List<ItemStack> { dummyItemStack },
			null,
			Recipe.CraftingType.Cooking
		);

		foreach (var recipe in dummyRecipes)
		{
			GD.Print(recipe.RecipeCraftingType);
		}

		// 4. Creating a custom Recipe to show how all of it works
		var customRecipe = new Recipe(
			new List<IIngredient>
			{
				new ComponentList { new Basil() },
				new ComponentList { new Basil() },
			},
			ItemDatabase.Instance.CreateItemStack("GameEndingNuke"),
			Recipe.CraftingType.Unspecified
		);

		var temp = ItemDatabase.Instance.CreateItemStack("BasilLeaf");
		GD.Print(customRecipe.CraftResult(new List<ItemStack> { temp }));

		//--------------------------------------------------------------------------------------------------------------------------------//
		// Some Tests

		GD.Print("---Tests---");
		{
			// Should fail, when not enough ingredients
			var recipe = new Recipe(
				new List<IIngredient>
				{
					new ComponentList { new Basil() },
					new ComponentList { new Basil() },
				},
				ItemDatabase.Instance.CreateItemStack("GameEndingNuke"),
				Recipe.CraftingType.Unspecified
			);

			var basil = ItemDatabase.Instance.CreateItemStack("BasilLeaf");
			Assert.AssertNull(recipe.CraftResult(new List<ItemStack> { basil }), "This should be null");
			GD.Print("Test 1 passed");
		}

		{
			// Should fail, when not wrong ingredients
			var recipe = new Recipe(
				new List<IIngredient>
				{
					new ComponentList { new Basil() },
					new ComponentList { new Decoration(), new Antioxidant() },
				},
				ItemDatabase.Instance.CreateItemStack("GameEndingNuke"),
				Recipe.CraftingType.Unspecified
			);

			var basil = ItemDatabase.Instance.CreateItemStack("BasilLeaf");
			Assert.AssertNull(recipe.CraftResult(new() { basil, basil }));
			GD.Print("Test 2 passed");
		}

		{
			// Should pass, with two matching ingredients
			var recipe = new Recipe(
				new List<IIngredient>
				{
					new ComponentList { new Basil() },
					new ComponentList { new Basil() },
				},
				ItemDatabase.Instance.CreateItemStack("GameEndingNuke"),
				Recipe.CraftingType.Unspecified
			);

			var basil = ItemDatabase.Instance.CreateItemStack("BasilLeaf");
			Assert.AssertNotNull(recipe.CraftResult(new() { basil, basil }));
			GD.Print("Test 3 passed");
		}

		// Result should remove component from result
		{
			var recipe = new Recipe(
				new List<IIngredient> { new ComponentList { new Basil() } },
				null,
				new ComponentList { new Leaf() },
				Recipe.CraftingType.Unspecified
			);

			var basil = ItemDatabase.Instance.CreateItemStack("BasilLeaf");
			var result = recipe.CraftResult(new() { basil });
			Assert.AssertNotNull(result);
			Assert.AssertNull(result.Components.FirstOrDefault(c => c.GetType() == typeof(Leaf)), "Should not have leaf component");
			GD.Print("Test 4 passed");
		}

		// Result should keep all component from result since removal does not match
		{
			var recipe = new Recipe(
				new List<IIngredient> { new ComponentList { new Basil() } },
				null,
				new ComponentList { new Nuke() },
				Recipe.CraftingType.Unspecified
			);

			var basil = ItemDatabase.Instance.CreateItemStack("BasilLeaf");
			var result = recipe.CraftResult(new() { basil });
			Assert.AssertNotNull(result);
			Assert.AssertTrue(
				result.Components.All(resComp => basil.Components.FirstOrDefault(comp => comp.GetType() == resComp.GetType()) != null),
				"Should have leaf component"
			);
			GD.Print("Test 5 passed");
		}

		var basilComp = new Basil();
		var dupe = basilComp.Clone();
		GD.Print(basilComp);
		GD.Print(dupe);
		Assert.AssertTrue(dupe.GetType() == typeof(Basil));

		{
			var coal = new ItemStack()
			{
				Id = "coal",
				Name = "Coal",
				Category = ItemCategory.Material,
			};
			var stick = new ItemStack()
			{
				Id = "stick",
				Name = "Stick",
				Category = ItemCategory.Material,
			};
			var torch = new ItemStack()
			{
				Id = "torch",
				Name = "Torch",
				Category = ItemCategory.Material,
			};
			var torchRecipe = new Recipe(new List<IIngredient> { new ItemId(coal.Id), new ItemId(stick.Id) }, torch);

			{
				var recipes = new List<Recipe>() { torchRecipe }
					.Concat(ItemDatabase.Instance.Recipes)
					.ToList();
				var res = ItemDatabase.Instance.GetAllRecipesWithItemStacks(new() { stick }, recipes);
				Assert.AssertTrue(res.Contains(torchRecipe));
			}
			{
				var recipes = new List<Recipe>() { torchRecipe }; //.Concat(ItemDatabase.Instance.Recipes).ToList();
				var res = ItemDatabase.Instance.GetAllRecipesWithItemStacks(new() { stick, stick }, recipes);
				Assert.AssertTrue(res.Count == 0, "Expected no recipes for stickx2");
			}
			{
				var recipes = new List<Recipe>() { torchRecipe }; //.Concat(ItemDatabase.Instance.Recipes).ToList();
				var basil = ItemDatabase.Instance.CreateItemStack("BasilLeaf");
				var res = ItemDatabase.Instance.GetAllRecipesWithItemStacks(new() { basil, stick }, recipes);
				Assert.AssertTrue(res.Count == 0, "Expected no recipes stick and basil");
			}
		}

		{
			GD.Print("----------TEST---------");
			var basil = new Basil();
			var basil1 = new Basil();
			GD.Print(basil.Equals(basil1)); // should be true

			var lavender = new Lavender();
			GD.Print(basil.Equals(lavender)); // should be false
		}
		*/
	}
}
