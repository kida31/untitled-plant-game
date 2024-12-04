using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Inventory.PlayerInventory.Views;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public partial class TabsController : Control
{

	[Export] private InventoryView[] _tabs;
	public List<ICategoryTab> Categories { get; private set; }
	private InventoryItemView _potentialItemSlot;
	
	public override void _EnterTree()
	{
		Categories = GetChildrenOfType<ICategoryTab>();
	}

	public override void _Ready()
	{
		EventBus.Instance.OnTabsUpdate += AddItemToCorrespondingTab;
		EventBus.Instance.OnInventoryItemMove += DropInventoryItemToNewSlot; // different

		EventBus.Instance.OnSetItemSlot += SetPotentialItemSlot;
		EventBus.Instance.OnGetItemSlot += GetPotentialItemSlot;

		_tabs = GetChildren().OfType<InventoryView>().ToArray();
		
		foreach (var tab in Categories)
		{
			tab.SetTabInventorySize(15);
		}
	}
	
	public void SetInventories(List<Inventory> inventories)
	{
		for (int i = 0; i < inventories.Count; i++)
		{
			var inv = inventories[i];
			var cat = Categories[i];
			cat.UpdateTabUi(inv.GetItems()[0]); // TODO make this a list
		}
		
		foreach (var child in GetChildren())
		{
			if (child is InventoryView invView)
			{
				invView.UpdateInventory(inventories[0]);
			}	
		}
	}
	
	/**
	 * Checks if first-degree nodes are of the type "ICategoryTab" and returns them.
	 */
	private List<T> GetChildrenOfType<T>()
		where T : ICategoryTab
	{
		var childrenOfType = new List<T>();
		for (var i = 0; i < this.GetChildCount(); i++)
		{
			var child = this.GetChild(i);
			if (child is T typedChild)
			{
				childrenOfType.Add(typedChild);
			}
		}

		return childrenOfType;
	}
	
	[Obsolete]
	private void AddItemToCorrespondingTab(ItemStack item)
	{
		Categories.OfType<SeedsTab>().FirstOrDefault()?.UpdateTabUi(item);
		// switch (item.GetItemType())
		// {
		// 	case HerbCategory:
		// 		Categories.OfType<HerbsTab>().FirstOrDefault()?.UpdateTabUi(item);
		// 		break;
		// 	case MedicineCategory:
		// 		Categories.OfType<MedicineTab>().FirstOrDefault()?.UpdateTabUi(item);
		// 		break;
		// 	case SeedCategory:
		// 		Categories.OfType<SeedsTab>().FirstOrDefault()?.UpdateTabUi(item);
		// 		break;
		// }
	}

	[Obsolete]
	private void DropInventoryItemToNewSlot(ItemStack itemStack, InventoryItemView inventoryItemView)
	{
		Categories.OfType<SeedsTab>().FirstOrDefault()?.DropInventoryItemToNewSlot(itemStack, inventoryItemView.Id);

	}

	private void SetPotentialItemSlot(InventoryItemView inventoryItemView)
	{
		_potentialItemSlot = inventoryItemView;
	}

	private InventoryItemView GetPotentialItemSlot()
	{
		return _potentialItemSlot;
	}
}
