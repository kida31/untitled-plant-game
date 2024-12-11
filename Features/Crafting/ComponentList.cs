using System.Collections.Generic;
using untitledplantgame.Item;

namespace untitledplantgame.Crafting;

public class ComponentList : List<AComponent>, IRecipeFilterPart
{
	public ComponentList(List<AComponent> components = null)
	{
		foreach (var component in components ?? new List<AComponent>())
		{
			Add(component);
		} 
	}
}
