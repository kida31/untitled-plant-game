using Godot;
using Godot.Collections;

namespace untitledplantgame.Inventory.GeneralInventory;

public interface ICategoryTab
{
	
	string GetICategoryTabName();
	
	Array<Node> GetItemsInCategoryTab();

}
