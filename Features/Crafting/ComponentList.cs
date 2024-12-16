using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Inventory;
using untitledplantgame.Item;

namespace untitledplantgame.Crafting;

public class ComponentList : List<AComponent>, IIngredient
{
	public ComponentList(List<AComponent> components = null)
	{
		foreach (var component in components ?? new List<AComponent>())
		{
			Add(component);
		} 
	}

	public bool IsValidIngredient(IItemStack itemStack)
	{
		if (itemStack == null)
		{
			return false;
		}
		
		if (itemStack.Components.Count < Count)
		{
			return false;
		}
		
		// TODO: May need more specific matching logic
		foreach (var requiredComponent in this)
		{
			var matchingComponent = itemStack.Components.FirstOrDefault(c => c.GetType() == requiredComponent.GetType());
			if (matchingComponent == null)
			{
				return false;
			}
		}

		return true;
	}
}
