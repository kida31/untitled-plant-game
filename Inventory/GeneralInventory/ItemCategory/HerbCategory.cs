using untitledplantgame.Inventory.GeneralInventory;

public class HerbCategory : IItemCategory
{
	
	public string GetIItemCategoryName()
	{
		return "Herb";
	}

	public IItemCategory GetIItemCategory()
	{
		return this;
	}
	
	//add this category to ZA LIST (resource)
}
