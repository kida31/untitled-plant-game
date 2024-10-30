using Godot;
using untitledplantgame.Inventory.GeneralInventory;

public partial class IuiInventoryItem : Node, I_UI_InventoryItem
{
	private IItemCategory ItemCategory;
	
	public override void _Ready()
	{
		//TODO make category variable
		ItemCategory = new HerbCategory();
	}
	
	public override void _Process(double delta)
	{
	}

	public IItemCategory GetItemCategory()
	{
		return ItemCategory;
	}
}
