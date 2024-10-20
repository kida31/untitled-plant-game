using Godot;
using System;
using untitledplantgame.Inventory.GeneralInventory;

public class MedicineCategory : IItemCategory
{
	public string GetIItemCategoryName()
	{
		return "Medicine";
	}

	public IItemCategory GetIItemCategory()
	{
		return this;
	}
}
