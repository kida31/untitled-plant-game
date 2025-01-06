using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Crafting;
using untitledplantgame.Inventory;
using untitledplantgame.Item;
using untitledplantgame.Item.Components;

namespace untitledplantgame.Database;

[Singleton]
public class ItemDatabase
{
	public List<Recipe> Recipes { get; private set; }
	public List<ItemStack> ItemStacks { get; private set; }
	private static ItemDatabase _instance;

	private readonly Logger _logger = new("ItemDatabase");

	public static ItemDatabase Instance
	{
		get => _instance ??= new ItemDatabase();
		private set => _instance = value;
	}

	private ItemDatabase()
	{
		ItemStacks = FillDataBaseItemStackList();
		Recipes = FillDataBaseRecipeList();
	}

	//---Multithreading Testing---//
	/*
	 * That's definitely not safe. It probably should be done like this:
	 *
	   public async Task MyAsyncFunction(int i, int y)
		{
			// Start both tasks without awaiting immediately
			Task task1 = Task.Run(() => DoMultiThreading(i));
			Task task2 = Task.Run(() => DoMultiThreading(y));

			// Await both tasks to complete
			await Task.WhenAll(task1, task2);
		}
	 *
	 * This ensures that the game will only start once the Database is actually loaded, but there is no infrastructure for that, so it
	 * doesn't really matter for the time being
	 */
	public void MyAsyncFunction(int i, int y)
	{
		Task.Run(() => DoMultiThreading(i));
		Task.Run(() => DoMultiThreading(y));
	}

	private void DoMultiThreading(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GD.Print(i);
		}
	}

	//---Multithreading Testing---//

	/// <summary>
	/// Looks for an ItemStack with the specified ID. Returns a clone of the ItemStack.
	/// </summary>
	/// <param name="itemId"></param>
	/// <returns></returns>
	public ItemStack CreateItemStack(string itemId)
	{
		var item = ItemStacks.FirstOrDefault(itemStack => itemStack.Id == itemId)?.Clone();
		if (item == null)
		{
			_logger.Error("Item with ID: " + itemId + " does not exist in the Database.");
		}

		return item as ItemStack;
	}

	/// <summary>
	/// Returns a list of ItemStacks that contain the specified components.
	/// If the user throws two or more components into the specified list, the Database will search accordingly to that.
	/// I.e.: { AComponent, AComponent, AComponent } is NOT the same as { AComponent, AComponent }.
	/// This will only return exact matches.
	/// </summary>
	/// <param name="components"></param>
	/// <returns></returns>
	public List<ItemStack> GetItemStacksWithSpecifiedComponents(List<AComponent> components)
	{
		var specificItemStack = new List<ItemStack>();
		var group1 = components.GroupBy(item => item.GetType()).ToDictionary(g => g.Key, g => g.Count());

		foreach (var itemStack in ItemStacks)
		{
			var group2 = itemStack.Components.GroupBy(item => item.GetType()).ToDictionary(g => g.Key, g => g.Count());

			if (group1.Count == group2.Count && group1.All(kvp => group2.TryGetValue(kvp.Key, out var count) && count == kvp.Value))
			{
				specificItemStack.Add(itemStack);
			}
		}

		return specificItemStack;
	}

	/// <summary>
	/// Returns a list of ItemStacks that contain the specified components.
	/// Items that contain more components than the specified
	/// ones will also be returned.
	/// </summary>
	/// <param name="components"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public ItemStack GetItemStacksWithAtLeastThoseComponents(List<AComponent> components)
	{
		throw new NotImplementedException();
	}

	//---Get Recipes---//
	/*
	 * This method assumes the following: The user will NEVER provide MORE ItemStack than the Recipe needs (Minecraft Crafting Bench).
	 * But the user will get a list of potential Recipes that require at least the provided ItemStacks, but also the additional ones.
	 * (Minecraft Inventory Helper)
	 *
	 * Crossier's Note: I apologize in advance for the abomination I produced here.
	 */

	// Additional Method: Get Recipes with EXACT amount of itemStacks.
	public List<Recipe> GetAllRecipesWithItemStacks(List<ItemStack> itemStacks, List<Recipe> externalRecipeList)
	{
		var recipeSearchList = externalRecipeList ?? Recipes;

		bool UsesIngredients(Recipe recipe, IReadOnlyCollection<ItemStack> items)
		{
			if (items.Count > recipe.Ingredients.Count)
			{
				return false;
			}

			// Check Id match first then components
			var availableIngredients = new List<IIngredient>()
				.Concat(recipe.Ingredients.OfType<ItemId>())
				.Concat(recipe.Ingredients.OfType<ComponentList>())
				.ToList();

			// Find a use for each item while not filling the same ingredient twice
			// Remark: May need to check by ingredient order instead of items, in case it 'takes away' an ingredient
			return items.All(item =>
			{
				var matchingIngredient = availableIngredients.FirstOrDefault(ingredient => ingredient.IsValidIngredient(item));
				if (matchingIngredient == null)
				{
					return false;
				}

				availableIngredients.Remove(matchingIngredient); // Item cannot be used for same ingredient
				return true;
			});
		}

		return recipeSearchList.Where(recipe => UsesIngredients(recipe, itemStacks)).ToList();
	}

	public List<Recipe> GetAllRecipesWithItemStacksAndCraftingType(
		List<ItemStack> itemStacks,
		List<Recipe> externalRecipeList,
		Recipe.CraftingType craftingType
	)
	{
		var recipesWithMatchingCraftingType = new List<Recipe>();
		var recipeSearchList = externalRecipeList ?? Recipes;

		foreach (var recipe in recipeSearchList)
		{
			if (recipe.RecipeCraftingType == craftingType)
			{
				recipesWithMatchingCraftingType.Add(recipe);
			}
		}

		return GetAllRecipesWithItemStacks(itemStacks, recipesWithMatchingCraftingType);
	}

	//---Get Recipes---//


	//------------------------------------------------------------------------------------------------------------------------------------//
	/*
	 * This method has no inherent purpose. It only exists to make the constructor more user-friendly and smaller (as in fewer lines).
	 */
	private List<Recipe> FillDataBaseRecipeList()
	{
		return new List<Recipe>();
	}

	/*
	 * This method has no inherent purpose. It only exists to make the constructor more user-friendly and smaller (as in fewer lines).
	 *
	 * NOTE: This method assumes that every single item has a unique ID. The method can have unpredictable consequences if two identical
	 * items exist in it.
	 */
	private List<ItemStack> FillDataBaseItemStackList()
	{
		return new List<ItemStack>
		{
			new()
			{
				Id = "unknownSeed",
				Name = "Unknown Seed",
				ToolTipDescription = "An unknown seed.",
				WikiDescription = "An unknown seed. Plant it and water it regularly to find out what it produces!",
				Icon = GD.Load<Texture2D>("res://Assets/Items/chubery_harvested.png"), //TODO: add seed icon
				Category = ItemCategory.Seed,
				BaseValue = 5,
			},
			new()
			{
				Id = "chuberrySeed",
				Name = "Chuberry Seed",
				ToolTipDescription = "The seeds of a chubery plant.",
				WikiDescription = "The seeds of a chubery plant. They have to be planted in soil and watered regularly to reward with tasty berries.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/chubery_harvested.png"), //TODO: add seed icon
				Category = ItemCategory.Seed,
				BaseValue = 5,
				RelatedItemIds = new Array<string> { "chuberryFruit", "chuberryDried" },
				Components = new Array<AComponent> {new SeedComponent("Chuberry")},
			},
			new()
			{
				Id = "chuberryFruit",
				Name = "Chuberry Fruit",
				ToolTipDescription = "The fruits of a chubery plant.",
				WikiDescription = "The berries of a chubery plant. While the plant itself looks quite gnarly, the berries are surprisingly juicy. It can be pressed into juice, though most people just dry them and eat them as a snack or ingredient in cooking and baking. It helps boost the immune system, so it’s a widely used plant by many in Tawas. Use it preventive or as an acute immune booster. ",
				Icon = GD.Load<Texture2D>("res://Assets/Items/chubery_harvested.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 5,
				RelatedItemIds = new Array<string> { "chuberrySeed", "chuberryDried" },
			},
			new()
			{
				Id = "chuberryDried",
				Name = "Dried Chuberry",
				ToolTipDescription = "The dried berries of a chubery plant.",
				WikiDescription = "The dried berries of a chubery plant. Since berries can’t be kept forever and these particular ones are liked to be eaten all year round, the practice of drying them became a standard. ",
				Category = ItemCategory.Medicine,
				Icon = GD.Load<Texture2D>("res://Assets/Items/chubery_harvested.png"), //TODO: add dried icon
				MaxStackSize = 64,
				BaseValue = 5,
				Amount = 1,
				//Components = new Array<AComponent> { new Basil(), new Leaf(), new Spice() },
				RelatedItemIds = new Array<string> { "cuberrySeed", "chuberryFruit" },
			},
			new()
			{
				Id = "drupoleaumSeed",
				Name = "Drupoleaum Seed",
				ToolTipDescription = "The seeds of a Drupoleaum plant.",
				WikiDescription = "The seeds of a Drupoleaum plant. They have to be planted in soil and watered regularly. It will grow up a stalk.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Drupoleaum_Flowers.png"), //TODO: add seed icon
				Category = ItemCategory.Seed,
				BaseValue = 5,
				RelatedItemIds = new Array<string> { "drupoleaumFlower", "drupoleaumFruits" },
			},
			new()
			{
				Id = "drupoleaumFlower",
				Name = "Drupoleaum Flower",
				ToolTipDescription = "The flowers of a Drupoleaum plant.",
				WikiDescription = "The flowers of a drupoleaum plant. Traditionally the people of Liyor held a flower festival to celebrate the blooming of the first Drupolearum flower. As the wild variations of drupoleaum vined up trees, a popular game was to find the highest growing flower and offer it to the goddess’s shrine. Nowadays, the flowers are a popular tea variant and help with finding sleep.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Drupoleaum_Flowers.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 5,
				RelatedItemIds = new Array<string> { "drupoleaumSeed", "drupoleaumFruits" },
			},
			new()
			{
				Id = "drupoleaumFruits",
				Name = "Drupoleaum Fruit",
				ToolTipDescription = "The fruits of a Drupoleaum plant.",
				WikiDescription = "The berries of a drupoleaum plant. For a long time, the use of drupoleaum berries wasn’t common, as the majority of flowers got picked before ever developing into fruits. Just recently their anti-inflammatory effects have become known which led to a high demand for berries after the Big Flooding.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Drupoleaum_Fruits.png"), 
				Category = ItemCategory.Medicine,
				BaseValue = 5,
				RelatedItemIds = new Array<string> { "drupoleaumFlower", "drupoleaumSeed" },
			},
			new()
			{
				Id = "licarySeed",
				Name = "Licary Seed",
				ToolTipDescription = "The seeds of a Licary plant.",
				WikiDescription = "The seeds of a Licary plant. They have to be planted in soil and watered regularly to reward you with multiple harvestable Items.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/licary_flowers.png"), //TODO add icon
				Category = ItemCategory.Seed,
				BaseValue = 5,
				RelatedItemIds = new Array<string> { "licaryFlowers", "licaryFlowers", "licaryFruit", "licaryLeaf", "licaryLeafDried" },
			},
			new()
			{
				Id = "licaryFlowers",
				Name = "Licary Flowers",
				ToolTipDescription = "The flowers of a Licary plant.",
				WikiDescription = "The flowers of a licary plant. Their four bright yellow leaves often get associated with the power of the sun so a tea made out of these flowers is a popular morning drink. Whenever the colder days arrive, people stock up on these flowers to always have the sun around.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/licary_flowers.png"), 
				Category = ItemCategory.Medicine,
				BaseValue = 5,
				RelatedItemIds = new Array<string> { "licarySeed", "licaryFruit", "licaryLeaf", "licaryLeafDried" },
			},
			new()
			{
				Id = "licaryFruit",
				Name = "Licary Fruit",
				ToolTipDescription = "The fruits of a Licary plant.",
				WikiDescription = "The fruits of a licary plant. The small but bright orange fruit has a hard outer skin that makes it uncomfortable to eat. It’s anti-oxidant effect makes it a popular juice though. Parents usually pack small bottles of Licary juice as lunch drinks for their kids at school.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/licary_harvested.png"), 
				Category = ItemCategory.Medicine,
				BaseValue = 5,
				RelatedItemIds = new Array<string> { "licarySeed", "licaryFlowers", "licaryLeaf", "licaryLeafDried"},
			},
			new()
			{
				Id = "licaryLeaf",
				Name = "Licary Leaves",
				ToolTipDescription = "The leaves of a Licary plant.",
				WikiDescription = "The leaves of a licary plant. While it’s other produce shines bright, the pain-reducing effect of the licary leaves is often overlooked. A tea made from this leaf was first seen used by the women of the Tsaa tribe to ease menstrual pain.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/licary_leaves.png"), 
				Category = ItemCategory.Medicine,
				BaseValue = 5,
				RelatedItemIds = new Array<string> { "licarySeed", "licaryFlowers", "licaryFruit", "licaryLeafDried"},
			},
			new()
			{
				Id = "licaryLeafDried",
				Name = "Dried Licary Leaf",
				ToolTipDescription = "The dried leaves of a Licary plant.",
				WikiDescription = "The dried leaves of a licary plant. They are being used to brew a pain reducing tea. Dried leaves are preferred because they can be stored longer without going bad.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/licary_leaves.png"), 
				Category = ItemCategory.Medicine,
				BaseValue = 5,
				RelatedItemIds = new Array<string> { "licarySeed", "licaryFlowers", "licaryFruit", "licaryLeaf"},
			},
		};
	}
	//------------------------------------------------------------------------------------------------------------------------------------//
}
