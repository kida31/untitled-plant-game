using Godot;
using System;
using untitledplantgame.Inventory.GeneralInventory;

public partial class InventoryItem : Node, IInventoryItem
{
	[Export] private string Name;
	private IItemCategory ItemCategoryName;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//TODO make category variable
		ItemCategoryName = new HerbCategory();
		GD.Print(ItemCategoryName.GetIItemCategory());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
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
