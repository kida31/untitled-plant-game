using System.Collections.Generic;
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
}
