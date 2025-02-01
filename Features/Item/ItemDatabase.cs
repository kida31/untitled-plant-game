using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Crafting;
using untitledplantgame.Inventory;
using untitledplantgame.Item.Components;
using untitledplantgame.Medicine;
using untitledplantgame.Plants;

namespace untitledplantgame.Item;

[Singleton]
public class ItemDatabase
{
	public List<Recipe> Recipes { get; private set; }

	public List<IItemStack> GetAllItems()
	{
		return _itemStacks.Select(it => it?.Clone()).ToList();
	}

	private static ItemDatabase _instance;
	private readonly Logger _logger = new("ItemDatabase");
	private readonly List<IItemStack> _itemStacks;

	public static ItemDatabase Instance
	{
		get => _instance ??= new ItemDatabase();
		private set => _instance = value;
	}

	private ItemDatabase()
	{
		_itemStacks = FillDataBaseItemStackList();
		Recipes = FillDataBaseRecipeList();
	}


	/// <summary>
	/// Looks for an ItemStack with the specified ID. Returns a clone of the ItemStack.
	/// </summary>
	/// <param name="itemId"></param>
	/// <param name="amount"></param>
	/// <returns></returns>
	public IItemStack CreateItemStack(string itemId, int amount = 1)
	{
		var item = _itemStacks.FirstOrDefault(itemStack => itemStack.Id == itemId)?.Clone();
		if (item == null)
		{
			_logger.Error("Item with ID: " + itemId + " does not exist in the Database.");
			return null;
		}

		item.Amount = amount;

		return item;
	}

	/// <summary>
	/// Returns a list of ItemStacks that contain the specified components.
	/// If the user throws two or more components into the specified list, the Database will search accordingly to that.
	/// I.e.: { AComponent, AComponent, AComponent } is NOT the same as { AComponent, AComponent }.
	/// This will only return exact matches.
	/// </summary>
	/// <param name="components"></param>
	/// <returns></returns>
	public List<IItemStack> GetItemStacksWithSpecifiedComponents(List<AComponent> components)
	{
		var specificItemStack = new List<IItemStack>();
		var group1 = components.GroupBy(item => item.GetType()).ToDictionary(g => g.Key, g => g.Count());

		foreach (var itemStack in _itemStacks)
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
	public IItemStack GetItemStacksWithAtLeastThoseComponents(List<AComponent> components)
	{
		throw new NotImplementedException();
	}

	public List<Recipe> GetAllRecipesWithItemStacks(List<IItemStack> itemStacks, List<Recipe> externalRecipeList)
	{
		var recipeSearchList = externalRecipeList ?? Recipes;

		bool UsesIngredients(Recipe recipe, IReadOnlyCollection<IItemStack> items)
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
		List<IItemStack> itemStacks,
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

	private List<Recipe> FillDataBaseRecipeList()
	{
		return new List<Recipe>();
	}


	private List<IItemStack> FillDataBaseItemStackList()
	{
		return new List<IItemStack>
		{
			// TODO: This will be implemented as a separate feature
			// new ItemStack()
			// {
			// 	Id = "unknownSeedTemplate",
			// 	Name = "Unknown Seed",
			// 	ToolTipDescription = "An unknown seed.",
			// 	WikiDescription = "An unknown seed. Plant it and water it regularly to find out what it produces!",
			// 	Icon = GD.Load<Texture2D>("res://Assets/Items/chubery_harvested.png"),
			// 	Category = ItemCategory.Seed,
			// 	BaseValue = 5,
			// 	Components = new Array<AComponent> { new SeedComponent("") },
			// },
			new ItemStack()
			{
				Id = "chuberrySeed",
				Name = "Chuberry Seed",
				ToolTipDescription = "The seeds of a chuberry plant.",
				WikiDescription =
					"The seeds of a chuberry plant. They have to be planted in soil and watered regularly to reward with tasty berries.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Plants/chubery_seed.png"),
				Category = ItemCategory.Seed,
				BaseValue = 10,
				RelatedItemIds = new Array<string> {"chuberryFruit"},
				Components = new Array<AComponent> {new SeedComponent("Chuberry")}
			},
			new ItemStack()
			{
				Id = "chuberryFruit",
				Name = "Chuberry Fruit",
				ToolTipDescription = "The fruits of a chuberry plant.",
				WikiDescription =
					"The berries of a chuberry plant. While the plant itself looks quite gnarly, the berries are surprisingly juicy. It can be pressed into juice, though most people just dry them and eat them as a snack or ingredient in cooking and baking. " +
					"It helps boost the immune system, so it’s a widely used plant by many in Tawas. Use it preventive or as an acute immune booster. ",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Plants/chubery_harvested.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 10,
				RelatedItemIds = new Array<string> {"chuberrySeed"},
				Components = new()
				{
					new TagsComponent(TagsComponent.Tags.IsDrieable, TagsComponent.Tags.IsFruit),
					new HarvestedComponent("Chuberry", GrowthStage.Ripening),
					new MedicineComponent(new System.Collections.Generic.Dictionary<MedicinalEffect, int>
					{
						{MedicinalEffect.Warming, 1},
						{MedicinalEffect.WoundHealing, 2},
					}, new System.Collections.Generic.Dictionary<IllnessEffect, int>
					{
						{IllnessEffect.Heartburn, 2},
						{IllnessEffect.Diarrhea, 1}
					})
				}
			},
			new ItemStack
			{
				Id = "drupoleaumSeed",
				Name = "Drupoleaum Seed",
				ToolTipDescription = "The seeds of a Drupoleaum plant.",
				WikiDescription =
					"The seeds of a Drupoleaum plant. They have to be planted in soil and watered regularly. It will grow up a stalk.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Plants/Drupoleaum_seeds.png"),
				Category = ItemCategory.Seed,
				BaseValue = 100,
				RelatedItemIds = new Array<string> {"drupoleaumFlower", "drupoleaumFruits"},
				Components = new Array<AComponent>
				{
					new SeedComponent("Drupoleaum")
				}
			},
			new ItemStack
			{
				Id = "drupoleaumFlower",
				Name = "Drupoleaum Flower",
				ToolTipDescription = "The flowers of a Drupoleaum plant.",
				WikiDescription =
					"The flowers of a drupoleaum plant. Traditionally the people of Liyor held a flower festival to celebrate the blooming of the first Drupoleaum flower." +
					" As the wild variations of drupoleaum vined up trees, a popular game was to find the highest growing flower and offer it to the goddess’s shrine. Nowadays, the flowers are a popular tea variant and help with finding sleep.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Plants/Drupoleaum_Flowers.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 50,
				RelatedItemIds = new Array<string> {"drupoleaumSeed", "drupoleaumFruits"},
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsDrieable, TagsComponent.Tags.IsFlower),
					new HarvestedComponent("Drupoleaum", GrowthStage.Flowering),
					new MedicineComponent(new System.Collections.Generic.Dictionary<MedicinalEffect, int>
					{
						{MedicinalEffect.Cooling, 2},
						{MedicinalEffect.Calming, 1},
					}, new System.Collections.Generic.Dictionary<IllnessEffect, int>
					{
						{IllnessEffect.Heartburn, 2},
						{IllnessEffect.Nausea, 1}
					})
				}
			},
			new ItemStack
			{
				Id = "drupoleaumFruits",
				Name = "Drupoleaum Fruit",
				ToolTipDescription = "The fruits of a Drupoleaum plant.",
				WikiDescription =
					"The berries of a drupoleaum plant. For a long time, the use of drupoleaum berries was not common, as the majority of flowers got picked before ever developing into fruits." +
					" Just recently their anti-inflammatory effects have become known which led to a high demand for berries after the Big Flooding.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Plants/Drupoleaum_Fruits.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 100,
				RelatedItemIds = new Array<string> {"drupoleaumFlower", "drupoleaumSeed"},
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsDrieable, TagsComponent.Tags.IsFruit),
					new HarvestedComponent("Drupoleaum", GrowthStage.Ripening),
					new MedicineComponent(new System.Collections.Generic.Dictionary<MedicinalEffect, int>
					{
						{MedicinalEffect.Cooling, 2},
						{MedicinalEffect.Calming, 3},
						{MedicinalEffect.AntiInflammatory, 1}
					}, new System.Collections.Generic.Dictionary<IllnessEffect, int>
					{
						{IllnessEffect.Heartburn, 2},
						{IllnessEffect.Nausea, 1}
					})
				}
			},
			new ItemStack
			{
				Id = "licarySeed",
				Name = "Licary Seed",
				ToolTipDescription = "The seeds of a Licary plant.",
				WikiDescription =
					"The seeds of a Licary plant. They have to be planted in soil and watered regularly to reward you with multiple harvestable Items.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Plants/licary_seed.png"),
				Category = ItemCategory.Seed,
				BaseValue = 350,
				RelatedItemIds = new Array<string> {"licaryFlowers", "licaryFlowers", "licaryFruit"},
				Components = new Array<AComponent>
				{
					new SeedComponent("Licary")
				}
			},
			new ItemStack
			{
				Id = "licaryLeaves",
				Name = "Licary Leaves",
				ToolTipDescription = "The leaves of a Licary plant.",
				WikiDescription =
					"The leaves of a licary plant. The leaves are pretty bitter and cannot be eaten. People cannot eat it without first processing it. " +
					"Usually it is dried and then brewed into a tea. The tea is said to help with colds and flu.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Plants/licary_leaves.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 12,
				RelatedItemIds = new Array<string> {"licarySeed", "licaryFruit"},
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsDrieable, TagsComponent.Tags.IsFlower),
					new HarvestedComponent("Licary", GrowthStage.Flowering),
					new MedicineComponent(new System.Collections.Generic.Dictionary<MedicinalEffect, int>
					{
						{MedicinalEffect.Warming, 3},
						{MedicinalEffect.PainRelief, 1},
					}, new System.Collections.Generic.Dictionary<IllnessEffect, int>
					{
						{IllnessEffect.Indigestion, 3},
						{IllnessEffect.HeartAttack, 1}
					})
				}
			},
			new ItemStack
			{
				Id = "licaryFlowers",
				Name = "Licary Flowers",
				ToolTipDescription = "The flowers of a Licary plant.",
				WikiDescription =
					"The flowers of a licary plant. Their four bright yellow leaves often get associated with the power of the sun so a tea made out of these flowers is a popular morning drink. " +
					"Whenever the colder days arrive, people stock up on these flowers to always have the sun around.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Plants/licary_flowers.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 69,
				RelatedItemIds = new Array<string> {"licarySeed", "licaryFruit"},
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsDrieable, TagsComponent.Tags.IsFlower),
					new HarvestedComponent("Licary", GrowthStage.Flowering),
					new MedicineComponent(new System.Collections.Generic.Dictionary<MedicinalEffect, int>
					{
						{MedicinalEffect.Warming, 1},
						{MedicinalEffect.PainRelief, 2},
					}, new System.Collections.Generic.Dictionary<IllnessEffect, int>
					{
						{IllnessEffect.Migraine, 2},
						{IllnessEffect.HeartAttack, 1}
					})
				}
			},
			new ItemStack
			{
				Id = "licaryFruit",
				Name = "Licary Fruit",
				ToolTipDescription = "The fruits of a Licary plant.",
				WikiDescription =
					"The fruits of a licary plant. The small but bright orange fruit has a hard outer skin that makes it uncomfortable to eat." +
					" It’s anti-oxidant effect makes it a popular juice though. Parents usually pack small bottles of Licary juice as lunch drinks for their kids at school.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Plants/licary_harvested.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 420,
				RelatedItemIds = new Array<string> {"licarySeed", "licaryFlowers"},
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsDrieable, TagsComponent.Tags.IsFruit),
					new HarvestedComponent("Licary", GrowthStage.Ripening),
					new MedicineComponent(new System.Collections.Generic.Dictionary<MedicinalEffect, int>
					{
						{MedicinalEffect.Warming, 3},
						{MedicinalEffect.PainRelief, 1},
						{MedicinalEffect.AntiOxidant, 3}
					}, new System.Collections.Generic.Dictionary<IllnessEffect, int>
					{
						{IllnessEffect.Indigestion, 2},
						{IllnessEffect.HeartAttack, 1}
					})
				}
			},
			new ItemStack
			{
				Id = "dried_fruit",
				Name = "Dried Fruit",
				ToolTipDescription = "A dried fruit. I wonder what it tastes like.",
				WikiDescription = "A dried fruit. Drying fruits in the dehydrator will make them last longer and amplify their properties.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Plants/dried_chubery.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 0,
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsDried, TagsComponent.Tags.IsFruit)
				}
			},
			new ItemStack
			{
				Id = "dried_flower",
				Name = "Dried Flower",
				ToolTipDescription = "A dried flower. Still looks very pretty.",
				WikiDescription = "A dried flower. Drying flowers in the dehydrator will make them last longer and amplify their properties.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Plants/dried_drupoleaum.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 0,
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsDried, TagsComponent.Tags.IsFlower),
				},
			},
			new ItemStack
			{
				Id = "dried_leaves",
				Name = "Dried Leaf",
				ToolTipDescription = "Dried leaves. They seem very delicate.",
				WikiDescription = "Dried leaves. Drying leaves in the dehydrator will make them last longer and amplify their properties.",
				Icon = GD.Load<Texture2D>("res://Assets/Tilesets/Plant/DeadPlant.png"),
				Category = ItemCategory.Medicine,
				BaseValue = 0,
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsDried, TagsComponent.Tags.IsLeaf)
				},
			},
			new ItemStack
			{
				Id = "rubber_duck",
				Name = "Rubber Duck",
				ToolTipDescription = "A rubber duck. It squeaks when you squeeze it.",
				WikiDescription =
					"A random rubber duck, found in the wild. It’s a mystery how it got here. You can squeeze it and it will make a squeaky sound. Maybe it's a relic of the past?",
				Icon = GD.Load<Texture2D>("res://Assets/Items/Duck.png"),
				Category = ItemCategory.Material,
				BaseValue = 420,
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsPriceless, TagsComponent.Tags.IsWorthless, TagsComponent.Tags.IsMagical, TagsComponent.Tags.IsInedible)
				},
			},
			new ItemStack
			{
				Id = "blue_fish",
				Name = "Blue Fish",
				ToolTipDescription = "A blue fish. It smells like the ocean.",
				WikiDescription =
					"A fish with pretty blue scales. It smells like the ocean. It’s a popular ingredient in many dishes, but unfortunately, I can’t cook.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/FishingMyFishies/Fish1.png"),
				Category = ItemCategory.Material,
				BaseValue = 0,
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsEdible,TagsComponent.Tags.IsFish)
				},
			},
			new ItemStack
			{
				Id = "purple_fish",
				Name = "Purple Fish",
				ToolTipDescription = "A purple fish. It smells like the ocean.",
				WikiDescription =
					"A fish with pretty purple scales. It smells like the ocean. It’s a popular ingredient in many dishes, but unfortunately, I can’t cook.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/FishingMyFishies/Fish2.png"),
				Category = ItemCategory.Material,
				BaseValue = 0,
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsEdible,TagsComponent.Tags.IsFish)
				},
			},
			new ItemStack
			{
				Id = "lightblue_fish",
				Name = "Light Blue Fish",
				ToolTipDescription = "A light blue fish. It smells like the ocean.",
				WikiDescription =
					"A fish with scales like ice. It smells like the ocean. It’s a popular ingredient in many dishes, but unfortunately, I can’t cook.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/FishingMyFishies/Fish3.png"),
				Category = ItemCategory.Material,
				BaseValue = 0,
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsEdible,TagsComponent.Tags.IsFish)
				},
			},
			new ItemStack
			{
				Id = "pink_fish",
				Name = "Pink Fish",
				ToolTipDescription = "A pink fish. It smells like the ocean.",
				WikiDescription =
					"A fish with pinkish scales. It smells like the ocean. It’s a popular ingredient in many dishes, but unfortunately, I can’t cook.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/FishingMyFishies/Fish4.png"),
				Category = ItemCategory.Material,
				BaseValue = 0,
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsEdible, TagsComponent.Tags.IsFish)
				},
			},
			new ItemStack
			{
				Id = "green_fish",
				Name = "Green Fish",
				ToolTipDescription = "A green fish. It smells like the ocean.",
				WikiDescription =
					"A fish with light green scales. It smells like the ocean. It’s a popular ingredient in many dishes, but unfortunately, I can’t cook.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/FishingMyFishies/Fish6.png"),
				Category = ItemCategory.Material,
				BaseValue = 0,
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsEdible, TagsComponent.Tags.IsFish)
				},
			},
			new ItemStack
			{
				Id = "redspot_fish",
				Name = "Red Fish with Spots",
				ToolTipDescription = "A red fish. It has dark spots.",
				WikiDescription =
					"A red fish with spots. It looks a little curious. It must be a rarity! I want to keep it as a pet, I wonder where I could find an aquarium.",
				Icon = GD.Load<Texture2D>("res://Assets/Items/FishingMyFishies/Fish10.png"),
				Category = ItemCategory.Material,
				BaseValue = 0,
				Components = new Array<AComponent>
				{
					new TagsComponent(TagsComponent.Tags.IsEdible,TagsComponent.Tags.IsFish)
				},
			}
		};
	}
}
