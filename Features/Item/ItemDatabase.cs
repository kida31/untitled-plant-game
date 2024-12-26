using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Crafting;
using untitledplantgame.Inventory;
using untitledplantgame.Item.Components;
using untitledplantgame.Medicine;
using untitledplantgame.Plants;
using MedicineComponent = untitledplantgame.Item.Components.MedicineComponent;

namespace untitledplantgame.Item;

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
	public ItemStack CreateItemStack(string itemId, int amount = 1)
	{
		var item = ItemStacks.FirstOrDefault(itemStack => itemStack.Id == itemId)?.Clone();
		if (item == null)
		{
			_logger.Error("Item with ID: " + itemId + " does not exist in the Database.");
			return null;
		}

		item.Amount = amount;

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
				Id = "chuuberry",
				Name = "Chuuberry",
				Description = "A small, red berry that grows in the forest.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/chubery_harvested.png"),
				Category = ItemCategory.Plant,
				BaseValue = 5,
				RelatedItemIds = new Array<string> { "BasilLeaf" },
			},
			new()
			{
				Id = "dried_leaf",
				Name = "Dried ",
				Description = "Dried Leaf Template",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Dried_Leaf.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 5,
				Components = new Array<AComponent>
				{
					new DriedComponent(), //change this to tags?
				}
			},
			new()
			{
				Id = "dried_flower",
				Name = "Dried ",
				Description = "Dried Flower Template",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Dried_Flower.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 5,
				Components = new Array<AComponent>
				{
					new DriedComponent(), //change this to tags?
				}
			},
			new()
			{
				Id = "dried_fruit",
				Name = "Dried ",
				Description = "Dried Fruit Template",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Dried_Fruit.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 5,
				Components = new Array<AComponent>
				{
					new DriedComponent(), //change this to tags?
				}
			},
			//----------------------------------------------------------------------------------------------------------------------------//
			new(
				"firstItem",
				"theBestItem",
				null,
				"It's the first time.",
				ItemCategory.Plant,
				baseValue: 5,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"secondItem",
				"oakSapling",
				null,
				"A young oak tree ready to be planted.",
				ItemCategory.Plant,
				baseValue: 5,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"thirdItem",
				"pineCone",
				null,
				"A pine cone that might grow into a tree.",
				ItemCategory.Plant,
				baseValue: 3,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
		};
	}
	//------------------------------------------------------------------------------------------------------------------------------------//
}
