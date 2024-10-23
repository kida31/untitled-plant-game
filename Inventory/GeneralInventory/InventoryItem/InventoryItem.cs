using Godot;
using untitledplantgame.Inventory.GeneralInventory;

public partial class InventoryItem : Node, IInventoryItem
{
	[Export] private string Name;
	private IItemCategory ItemCategoryName;
	
	public override void _Ready()
	{
		//TODO make category variable
		ItemCategoryName = new HerbCategory();
		//GD.Print(ItemCategoryName.GetIItemCategory());
	}
	
	public override void _Process(double delta)
	{
	}

	public string GetInventoryItemName()
	{
		return Name;
	}

	public IItemCategory GetItemCategoryName()
	{
		return ItemCategoryName;
	}
}
