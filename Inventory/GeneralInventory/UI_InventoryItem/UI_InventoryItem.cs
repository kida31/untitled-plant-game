using Godot;
using untitledplantgame.Inventory.GeneralInventory.UI_ItemCategory;

public partial class UI_InventoryItem : Node, I_UI_InventoryItem
{
	private IItemCategory ItemCategory;
	
	public override void _Ready()
	{
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
