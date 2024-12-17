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
		return new List<Recipe>
		{
			// Generic: Turns single "Leaf" into "DriedLeaf"
			// Note for Testing: Doesn't work with Sunflower!
			new(
				new List<IIngredient> { new ComponentList { new Leaf() } },
				new ComponentList { new DriedLeaf() },
				new ComponentList { new Leaf() },
				Recipe.CraftingType.Drying
			),
			// Generic; Turns an item with an "Oil" and an item with an "Antioxidant" component into a normal item containing both.
			// Note for Testing: Just to show that we mix both components together without removing or changing anything.
			new(
				new List<IIngredient>
				{
					new ComponentList { new Oil() },
					new ComponentList { new Oil() },
					new ItemId("Sunflower"),
					new ComponentList { new Antioxidant() },
				},
				null,
				null,
				Recipe.CraftingType.Brewing
			),
			// Generic; Showcase of filtering for itemNames
			// Note for Testing: Searching for string and component!
			new(
				new List<IIngredient>
				{
					new ComponentList { new Basil() },
					new ItemId("MintLeaf"),
				},
				null,
				null,
				Recipe.CraftingType.Cooking
			),
			// Generic; Showcase dynamic nature of Recipes
			// Note for Testing: TACTICAL NUKE INCOMING. ÜÜEHH-ÜÜEHH-ÜÜEHH
			new(
				new List<IIngredient>
				{
					new ComponentList { new Basil() },
					new ComponentList { new Lavender() },
					new ComponentList { new Mint() },
					new ComponentList { new Rose() },
					new ComponentList { new Sunflower() },
				},
				CreateItemStack("GameEndingNuke"),
				Recipe.CraftingType.Unspecified
			),
		};
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
				Id = "BasilLeaf",
				Name = "Basil",
				Description = "Basil Basil Basil Basil Basil",
				Category = ItemCategory.Plant,
				MaxStackSize = 64,
				BaseValue = 5,
				Amount = 1,
				Components = new Array<AComponent> { new Basil(), new Leaf(), new Spice() },
				RelatedItemIds = new Array<string> { "chuuberry" },
			},
			new(
				"BasilLeaf",
				"Basil",
				null,
				"Basil Basil Basil Basil Basil",
				ItemCategory.Plant,
				baseValue: 5,
				components: new Array<AComponent> { new Basil(), new Leaf(), new Spice() }
			),
			new(
				"LavenderLeaf",
				"Lavender",
				null,
				"Lavender Lavender Lavender Lavender Lavender",
				ItemCategory.Plant,
				baseValue: 5,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent> { new Lavender(), new Leaf(), new Sweet() }
			),
			new(
				"MintLeaf",
				"Mint",
				null,
				"Mint Mint Mint Mint Mint",
				ItemCategory.Plant,
				baseValue: 5,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent> { new Mint(), new Leaf(), new Antioxidant() }
			),
			new(
				"RoseLeaf",
				"Rose",
				null,
				"Rose Rose Rose Rose Rose",
				ItemCategory.Plant,
				baseValue: 5,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent> { new Rose(), new Leaf(), new Decoration() }
			),
			new(
				"Sunflower",
				"Sunflower",
				null,
				"Sunflower Sunflower Sunflower Sunflower Sunflower",
				ItemCategory.Plant,
				baseValue: 5,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent> { new Sunflower(), new Oil() }
			),
			new(
				"GameEndingNuke",
				"Nuke",
				null,
				"Unlocks after a 30 kill gun streak and- wait, this isn't Call of Duty...?!",
				ItemCategory.Material,
				baseValue: 30,
				maxStackSize: 1,
				amount: 1,
				components: new Array<AComponent> { new Nuke() }
			),
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
			new(
				"fourthItem",
				"healingHerb",
				null,
				"A small herb known for its healing properties.",
				ItemCategory.Medicine,
				baseValue: 10,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"fifthItem",
				"aloeLeaf",
				null,
				"A leaf with soothing gel inside.",
				ItemCategory.Medicine,
				baseValue: 15,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"sixthItem",
				"lavender",
				null,
				"A fragrant plant used in remedies.",
				ItemCategory.Plant,
				baseValue: 8,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"seventhItem",
				"cactusFruit",
				null,
				"A fruit from a desert cactus.",
				ItemCategory.Plant,
				baseValue: 6,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"eighthItem",
				"bamboo",
				null,
				"A tall, fast-growing plant.",
				ItemCategory.Plant,
				baseValue: 7,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"ninthItem",
				"coalOre",
				null,
				"A chunk of coal ore.",
				ItemCategory.Material,
				baseValue: 20,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"tenthItem",
				"ironOre",
				null,
				"A chunk of iron ore.",
				ItemCategory.Material,
				baseValue: 25,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"eleventhItem",
				"clayLump",
				null,
				"A lump of soft, malleable clay.",
				ItemCategory.Material,
				baseValue: 12,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"twelfthItem",
				"saltRock",
				null,
				"A rock containing crystallized salt.",
				ItemCategory.Material,
				baseValue: 10,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"thirteenthItem",
				"spiderSilk",
				null,
				"Silky thread harvested from spiders.",
				ItemCategory.Material,
				baseValue: 18,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"fourteenthItem",
				"healingRoot",
				null,
				"A root with medicinal properties.",
				ItemCategory.Medicine,
				baseValue: 20,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"fifteenthItem",
				"gingerRoot",
				null,
				"A spicy root used for healing and cooking.",
				ItemCategory.Medicine,
				baseValue: 15,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"sixteenthItem",
				"peppermintLeaf",
				null,
				"A refreshing leaf with healing properties.",
				ItemCategory.Medicine,
				baseValue: 12,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"seventeenthItem",
				"ashWood",
				null,
				"Wood from an ash tree, useful for crafting.",
				ItemCategory.Material,
				baseValue: 14,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"eighteenthItem",
				"stoneBlock",
				null,
				"A basic stone block for building.",
				ItemCategory.Material,
				baseValue: 5,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"nineteenthItem",
				"wheatPlant",
				null,
				"A fully grown wheat plant.",
				ItemCategory.Plant,
				baseValue: 8,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"twentiethItem",
				"carrot",
				null,
				"A nutritious root vegetable.",
				ItemCategory.Plant,
				baseValue: 5,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"twentyFirstItem",
				"potato",
				null,
				"A starchy plant for cooking.",
				ItemCategory.Plant,
				baseValue: 4,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"twentySecondItem",
				"goldNugget",
				null,
				"A small piece of unrefined gold.",
				ItemCategory.Material,
				baseValue: 50,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"twentyThirdItem",
				"silverOre",
				null,
				"A chunk of silver ore.",
				ItemCategory.Material,
				baseValue: 40,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"twentyFourthItem",
				"oakLeaf",
				null,
				"A fresh leaf from an oak tree.",
				ItemCategory.Plant,
				baseValue: 3,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"twentyFifthItem",
				"rosePetal",
				null,
				"A petal from a beautiful rose.",
				ItemCategory.Plant,
				baseValue: 6,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"twentySixthItem",
				"mushroom",
				null,
				"An edible mushroom found in the forest.",
				ItemCategory.Plant,
				baseValue: 7,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"twentySeventhItem",
				"clover",
				null,
				"A lucky four-leaf clover.",
				ItemCategory.Plant,
				baseValue: 20,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"twentyEighthItem",
				"amberChunk",
				null,
				"A fossilized piece of tree resin.",
				ItemCategory.Material,
				baseValue: 30,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"twentyNinthItem",
				"dandelion",
				null,
				"A common yellow flower.",
				ItemCategory.Plant,
				baseValue: 2,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
			new(
				"thirtiethItem",
				"charcoal",
				null,
				"A lightweight material for fuel.",
				ItemCategory.Material,
				baseValue: 10,
				maxStackSize: 64,
				amount: 1,
				components: new Array<AComponent>()
			),
		};
	}
	//------------------------------------------------------------------------------------------------------------------------------------//
}
