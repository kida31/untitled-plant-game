namespace untitledplantgame.Crafting;

public class ItemId : IIngredient
{
	public string Id { get; set; }

	public ItemId(string id)
	{
		Id = id;
	}
}
