using Godot;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using untitledplantgame.EntityStatsDataContainer;
using untitledplantgame.Inventory.GeneralInventory.UI_ItemCategory;
using untitledplantgame.Inventory.GeneralInventory.UI_Tabs;
using untitledplantgame.Item;

namespace untitledplantgame.Inventory.GeneralInventory;

/*
 * Reminder: C# doesn't copy objects inside of lists, but rather creates an additional reference!
 */
public partial class GeneralInventoryLogic : Node
{
	private Array<DataContainer> _dataContainers = new (); 
	private List<ICategoryTab> _categories;
	
	public override void _Ready()
	{
		var eventBus = GetNode<EventBus>("/root/EventBus");
		eventBus.OnItemPickUp += AddItemToInventory;
		
		_categories = GetChildrenOfType<ICategoryTab>();
	}
	
	public override void _Process(double delta)
	{
	}
	
	/**
	 * Checks if first-degree nodes are of the type "ICategoryTab" and returns them.
	 */
	private List<T> GetChildrenOfType<T>() where T : ICategoryTab
	{
		List<T> childrenOfType = new List<T>();
		for (int i = 0; i < GetChildCount(); i++)
		{
			Node child = GetChild(i);
			if (child is T typedChild)
			{
				childrenOfType.Add(typedChild);
			}
		}
        
		return childrenOfType;
	}

	private void AddItemToInventory(InteractableItem item)
	{
		// Debug purpose
		/*
		foreach (var dContainer in _dataContainers)
		{
			GD.Print("Amount of total DataContainers: " + dContainer);
		}
		*/
		_dataContainers.Add(item.GetItemDataContainer());
		
		switch (item.GetICharacteristic())
		{
			case HerbCategory:
				_categories.OfType<HerbsTab>().FirstOrDefault()?.UpdateItemsInTab(item.GetItemDataContainer());
				break;
			case MedicineCategory:
				_categories.OfType<MedicineTab>().FirstOrDefault()?.UpdateItemsInTab(item.GetItemDataContainer());
				break;
			case SeedCategory:
				_categories.OfType<SeedsTab>().FirstOrDefault()?.UpdateItemsInTab(item.GetItemDataContainer());
				break;
		}


		foreach (var tab in _categories)
		{
			foreach (var data in tab.GetItemsInCategoryTab())
			{
				foreach (var stat in data.GetEntityBaseStats())
				{
					GD.Print("In the tab: \"" + tab.GetType() + 
					            "\" the stat: \"" + stat.StatType +
					            "\" has the total value:" + stat.GetModifiedStatValue());
				}
			}
		}
	}
}
