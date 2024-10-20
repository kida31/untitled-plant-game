using Godot;
using System;
using untitledplantgame.Inventory.GeneralInventory;

public class SeedCategory : IItemCategory
{
	public string GetIItemCategoryName()
	{
		return "Seed";
	}

	public IItemCategory GetIItemCategory()
	{
		return this;
	}
}
