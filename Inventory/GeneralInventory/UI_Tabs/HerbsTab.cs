using Godot;
using System;
using Godot.Collections;
using untitledplantgame.EntityStatsDataContainer;

namespace untitledplantgame.Inventory.GeneralInventory.UI_Tabs;

public partial class HerbsTab : Node, ICategoryTab
{
	[Export] public string HerbsTabName;
	[Export] public string HerbsTabDescription;
	
	private Array<DataContainer> _dataContainers = new ();  
	
	public override void _Ready()
	{
	}
	
	public override void _Process(double delta)
	{
	}

	public void UpdateItemsInTab(DataContainer herbItem)
	{
		_dataContainers.Add(herbItem);
	}

	public Array<DataContainer> GetItemsInCategoryTab()
	{
		return _dataContainers;
	}
}
