using System.Collections;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public interface IIngredient
{
	bool IsValidIngredient(IItemStack itemStack);
}
