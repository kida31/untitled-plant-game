using Godot;
using System;
using Godot.Collections;
using untitledplantgame.EntityStatsDataContainer;

namespace untitledplantgame.Inventory.GeneralInventory.UI_Tabs;


public partial class SeedsTab : Node, ICategoryTab
{
	[Export] public string SeedsTabName;
	[Export] public string SeedsTabDescription;
	
	private Array<DataContainer> _dataContainers = new ();  
	
	public override void _Ready()
	{
	}
	
	public override void _Process(double delta)
	{
	}
	
	public void UpdateItemsInTab(DataContainer seedsItem)
	{
		_dataContainers.Add(seedsItem);
	}
	
	public Array<DataContainer> GetItemsInCategoryTab()
	{
		return _dataContainers;
	}
}
