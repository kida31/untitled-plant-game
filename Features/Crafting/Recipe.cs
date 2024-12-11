using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public class Recipe
{
	public readonly List<IRecipeFilterPart> FilterParts;
	public MatchType RecipeMatchType { get; private set; }
	public CraftingType RecipeCraftingType { get; private set; }
	public ComponentList AdditionalComponentsInResultingItem { get; private set; }
	public ComponentList RemovedComponentsInResultingItem { get; private set; }
	public ItemStack ResultingItemStack;

	// Enums moved into Recipe class
	public enum MatchType
	{
		Generic,
		Specific
	}

	public enum CraftingType
	{
		Unspecified,
		Drying,
		Cooking,
		Brewing
	}

	// Primary constructor
	private Recipe(
		MatchType matchType,
		CraftingType craftingType,
		List<IRecipeFilterPart> filterLists,
		ComponentList additionalComponentsInResultingItem,
		ComponentList removedComponentsInResultingItem)
	{
		RecipeMatchType = matchType;
		RecipeCraftingType = craftingType;
		FilterParts = filterLists;
		AdditionalComponentsInResultingItem = additionalComponentsInResultingItem;
		RemovedComponentsInResultingItem = removedComponentsInResultingItem;
	}

	/*
	 * Constructor chaining
	 */
	public Recipe(
		CraftingType craftingType,
		List<IRecipeFilterPart> itemFilterList,
		ComponentList additionalComponentsInResultingItem,
		ComponentList removedComponentsInResultingItem
	) : this(
		MatchType.Generic,
		craftingType,
		itemFilterList,
		additionalComponentsInResultingItem,
		removedComponentsInResultingItem)
	{
		//TODO: Handle Logic here...
		//New Item needs to be crafted accordingly based on components of old item
	}

	public Recipe(
		CraftingType craftingType,
		List<IRecipeFilterPart> itemFilterList,
		ItemStack resultingItemStack
	) : this(
		MatchType.Specific,
		craftingType,
		itemFilterList,
		null,
		null
	)
	{
		ResultingItemStack = resultingItemStack;
	}

	public ItemStack GetRecipeResult()
	{
		return ResultingItemStack;
	}

	public ItemStack CraftResult(List<ItemStack> itemStacks)
	{
		if (itemStacks.Count > FilterParts.Count)
		{
			GD.Print("TOO MANY ITEMS PROVIDED!");
			return null;
		}
		
		var matchingRecipesWithComponents = FilterParts.Where(_ => 
			FilterParts.OfType<ComponentList>().Any(componentList =>
				componentList.Any(componentInFilterList =>
					itemStacks.Any(itemStack =>
						itemStack.Components.Any(componentInItemStack =>
							componentInFilterList.GetType() == componentInItemStack.GetType()
						)
					)
				)
			)
		).ToList();

		var list1Types = matchingRecipesWithComponents.Select(x => x.GetType()).OrderBy(type => type.FullName).ToList();
		var list2Types = FilterParts.Select(x => x.GetType()).OrderBy(type => type.FullName).ToList();

		return null;
	}
}
