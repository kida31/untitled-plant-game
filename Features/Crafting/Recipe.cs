using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Item;

namespace untitledplantgame.Crafting;

public class Recipe
{
	public readonly List<IRecipeFilterPart> FilterParts;
	public readonly ItemStack ResultingItemStack;
	public ResultType CraftResultType => ResultingItemStack != null ? ResultType.Specific : ResultType.Generic;
	//public MatchType RecipeMatchType { get; private set; } // Change Name
	public CraftingType RecipeCraftingType { get; private set; }
	public ComponentList AdditionalComponentsInResultingItem { get; private set; }
	public ComponentList RemovedComponentsInResultingItem { get; private set; }

	private readonly Logger _logger = new("Recipe");

	// Enums moved into Recipe class
	public enum ResultType
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
		ResultType resultType,
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
		ResultType.Generic,
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
		ResultType.Specific,
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
			_logger.Debug("Provided Items do not match recipe requirements.");
			return null;
		}

		if (!HasRequiredIngredients(itemStacks))
		{
			_logger.Debug("Item requirements not met.");
			return null;
		}

		// If the resulting ItemStack was directly specified, return that, else create dynamic result
		return ResultingItemStack ?? CreateDynamicCraftResult(itemStacks);
	}
	
	/// <summary>
	/// Craft the resulting ItemStack, Merge components. Craft resulting ItemStack based on components
	/// <list type="number">
	/// <item><description>Make id unique by adding them in a particular way</description></item>
	/// <item><description>Combine Names (rule outside this scope) ⇒ Default to add names together</description></item>
	/// <item><description>Icon (rule outside this scope) ⇒ Default to using first Icon provided</description></item>
	/// <item><description>Category (rule outside this scope) ⇒ Default to using the first category</description></item>
	/// <item><description>maxStackSize (rule outside this scope) ⇒ Default to using the first number provided</description></item>
	/// <item><description>baseValue (rule outside this scope) ⇒ Default to using the first number provided</description></item>
	/// <item><description>amount will always be one as a result</description></item>
	/// <item><description>components: will be added and compared via component-interface</description></item>
	/// </list>
	/// </summary>
	/// <param name="itemStacks"></param>
	/// <returns></returns>
	private ItemStack CreateDynamicCraftResult(List<ItemStack> itemStacks)
	{

		var newId = string.Join("_", itemStacks.Select(item => item.Id).OrderBy(s => s));
		var name = string.Join("-", itemStacks.Select(item => item.Id).OrderBy(s => s)); // TODO: Combine names
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

		// ------------------ Add components if they don't exist ------------------
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

		// ------------------ Remove components if they exist ------------------
		newComponents = new Array<AComponent>(newComponents
			.Where(c => RemovedComponentsInResultingItem.All(r => r.GetType() != c.GetType()))
			.ToArray());

		return new ItemStack(newId, name, icon, newDescription, newItemCategory, newMaxStackSize, baseValue, 1,
			newComponents);
	}

	/// <summary>
	/// Check if items fulfill ingredient requirements
	/// </summary>
	/// <param name="itemStacks"></param>
	/// <returns></returns>
	private bool HasRequiredIngredients(List<ItemStack> itemStacks)
	{
		if (itemStacks.Count != FilterParts.Count)
		{
			_logger.Debug("Item count does not match recipe requirements.");
			return false;
		}

		// Check specific items first, then component specifications
		// because component match may 'take away' from specific item requirement
		var availableItems = new List<ItemStack>(itemStacks);
		var hasSpecificallyRequiredItems = FilterParts
			.OfType<ItemId>()
			.All(ingredient =>
			{
				var item = availableItems.FirstOrDefault(i => i.Id.Equals(ingredient.Id));
				if (item == null)
				{
					return false;
				}

				availableItems.Remove(item);
				return true;
			});

		if (!hasSpecificallyRequiredItems)
		{
			_logger.Debug("Specific item requirements not met.");
			return false;
		}

		// Check whether items with components are covered
		var hasAllItemsWithComponents = FilterParts
			.OfType<ComponentList>()
			.All(compList =>
			{
				var itemWithAllRequiredComponents = availableItems.FirstOrDefault(item =>
				{
					var itemComponentTypes = item.Components.ToList();
					return compList
						.All(requiredComp => itemComponentTypes.Any(c => c.Equals(requiredComp)));
				});

				if (itemWithAllRequiredComponents == null)
				{
					return false;
				}

				availableItems.Remove(itemWithAllRequiredComponents);
				return true;
			});

		if (!hasAllItemsWithComponents)
		{
			_logger.Debug("Items with required components not met.");
			return false;
		}

		return true;
	}

	public override string ToString()
	{
		return
			$"Recipe{{FilterParts: {FilterParts}, ResultingItemStack: {ResultingItemStack}, RecipeMatchType: {CraftResultType}, RecipeCraftingType: {RecipeCraftingType}, AdditionalComponentsInResultingItem: {AdditionalComponentsInResultingItem}, RemovedComponentsInResultingItem: {RemovedComponentsInResultingItem}}}";
	}
}
