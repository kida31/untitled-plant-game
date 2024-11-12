namespace untitledplantgame.Inventory.Alt;

public class ItemCategory
{
	public static ItemCategory Plant = new ItemCategory("Plant");
	public static ItemCategory Material = new ItemCategory("Material");
	public static ItemCategory Medicine = new ItemCategory("Medicine");
	
	public string Name { get; }

	private ItemCategory(string name) => Name = name;
}
