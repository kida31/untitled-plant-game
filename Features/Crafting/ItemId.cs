using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public class ItemId : IIngredient
{
	public string Id { get; set; }

	public ItemId(string id)
	{
		Id = id;
	}

	public bool IsValidIngredient(IItemStack itemStack)
	{
		return itemStack?.Id == Id;
	}
}
