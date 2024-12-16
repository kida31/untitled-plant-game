using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Collections;
using untitledplantgame.Inventory;
using untitledplantgame.Item;

namespace untitledplantgame.Crafting;

public class Recipe
{
	public readonly List<IRecipeFilterPart> FilterParts;
	public readonly ItemStack ResultingItemStack;
	public MatchType RecipeMatchType => ResultingItemStack != null ? MatchType.Specific : MatchType.Generic; 
	//public MatchType RecipeMatchType { get; private set; } // Change Name
	public CraftingType RecipeCraftingType { get; private set; }
	public ComponentList AdditionalComponentsInResultingItem { get; private set; }
	public ComponentList RemovedComponentsInResultingItem { get; private set; }


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
		//RecipeMatchType = matchType;
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
		if (itemStacks.Count <= FilterParts.Count)
		{
			var numberOfItemsInRecipe = 0;
			foreach (var filterPart in FilterParts)
			{
				switch (filterPart)
				{
					case ItemId id:
						foreach (var itemStack in itemStacks)
						{
							if (itemStack.Id.Equals(id.Id))
							{
								numberOfItemsInRecipe++;
							}
						}

						break;
					case ComponentList list:
						foreach (var itemStack in itemStacks)
						{
							var minComponents = 0;
							foreach (var componentInItemStack in itemStack.Components)
							{
								foreach (var componentInRecipe in list)
								{
									if (componentInItemStack.GetType() == componentInRecipe.GetType())
									{
										minComponents++;
										break;
									}
								}
							}

							if (minComponents >= list.Count)
							{
								numberOfItemsInRecipe++;
							}
						}

						break;
					default:
						// With Logger
						GD.Print("The IRecipeFilterPart: " + filterPart + " of type " + filterPart.GetType() + " is not supported");
						break;
				}
			}

			// Safety check to make sure we use exactly the right number of items. 
			if (numberOfItemsInRecipe == FilterParts.Count)
			{
				if (ResultingItemStack != null)
				{
					// If the resulting ItemStack was directly specified
					return ResultingItemStack;
				}

				// If the resulting ItemStack should be crafted based on components. 
				/*
				 * 1. Make id unique by adding them in a particular way
				 * 2. Combine Names (rule outside this scope) ⇒ Default to add names together
				 * 3. Icon (rule outside this scope) ⇒ Default to using first Icon provided
				 * 4. Category (rule outside this scope) ⇒ Default to using the first category
				 * 5. maxStackSize (rule outside this scope) ⇒ Default to using the first number provided
				 * 6. baseValue (rule outside this scope) ⇒ Default to using the first number provided
				 * 7. amount will always be one as a result
				 * 8. components: will be added and compared via component-interface
				 */
				var newId = string.Join("_", itemStacks.Select(item => item.Id));
				var name = string.Join("-", itemStacks.Select(item => item.Id));
				var icon = itemStacks[0].Icon;
				var newDescription = itemStacks[0].Description;
				var newItemCategory = itemStacks[0].Category;
				var newMaxStackSize = itemStacks[0].MaxStackSize;
				var baseValue = itemStacks[0].BaseValue;
				// Make sure Godot does actual deep copies
				var newComponents = itemStacks[0].Components.Duplicate(true);

				foreach (var itemStack in itemStacks)
				{
					foreach (var aComponent in itemStack.Components)
					{
						var matchingComponent = newComponents.FirstOrDefault(c => c.GetType() == aComponent.GetType());

						if (matchingComponent != null)
						{
							// Combine components if a match is found
							matchingComponent.CombineComponent(aComponent);
						}
						else
						{
							// Add the component to ListA if no match is found
							newComponents.Add(aComponent);
						}
					}
				}

				if (AdditionalComponentsInResultingItem != null)
				{
					// Add extra components if they don't already exist
					foreach (var extra in AdditionalComponentsInResultingItem)
					{
						if (newComponents.All(component => component.GetType() != extra.GetType()))
						{
							newComponents.Add(extra);
						}
					}
				}

				// Add to remove list (if component even exists)
				var componentsToRemove = new ComponentList();
				if (RemovedComponentsInResultingItem != null)
				{
					foreach (var component in newComponents)
					{
						foreach (var toRemove in RemovedComponentsInResultingItem)
						{
							if (component.GetType() == toRemove.GetType())
							{
								componentsToRemove.Add(component);
								break;
							}
						}
					}
				}

				// Actually remove the components after identifying them
				foreach (var toRemove in componentsToRemove)
				{
					newComponents.Remove(toRemove);
				}

				return new ItemStack(newId, name, icon, newDescription, newItemCategory, newMaxStackSize, baseValue, 1, newComponents);
			}

			// With Logger
			GD.PrintErr("Incorrect ItemStacks for this recipe where provided!");
		}

		// With Logger
		GD.Print("Provided Items do not match recipe requirements.");
		return null;
	}

	public override string ToString()
	{
		return
			$"Recipe{{FilterParts: {FilterParts}, ResultingItemStack: {ResultingItemStack}, RecipeMatchType: {RecipeMatchType}, RecipeCraftingType: {RecipeCraftingType}, AdditionalComponentsInResultingItem: {AdditionalComponentsInResultingItem}, RemovedComponentsInResultingItem: {RemovedComponentsInResultingItem}}}";
	}
}
