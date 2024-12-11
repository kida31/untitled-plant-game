namespace untitledplantgame.Crafting;

public class ItemId : IRecipeFilterPart
{
	public string Id { get; set; }

	public ItemId(string id)
	{
		Id = id;
	}
}
