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
	}
}
