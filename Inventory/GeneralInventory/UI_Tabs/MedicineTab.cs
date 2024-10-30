using Godot;
using System;
using Godot.Collections;
using untitledplantgame.EntityStatsDataContainer;

namespace untitledplantgame.Inventory.GeneralInventory.UI_Tabs;

public partial class MedicineTab : Node, ICategoryTab
{
	[Export] public string MedicineTabName;
	[Export] public string MedicineTabDescription;
	
	private Array<DataContainer> _dataContainers = new ();  
	
	public override void _Ready()
	{
	}
	
	public override void _Process(double delta)
	{
	}
	
	public void UpdateItemsInTab(DataContainer medicineItem)
	{
		_dataContainers.Add(medicineItem);
	}

	public Array<DataContainer> GetItemsInCategoryTab()
	{
		return _dataContainers;
	}
}
