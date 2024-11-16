using Godot;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Event;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Item;
using untitledplantgame.Item.ItemTypes;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public partial class TabsController : Control
{
	[Export] private PanelContainer _rightPanelForDetailedItemView;
	[Export] private PackedScene _detailedItemView;
	public List<ICategoryTab> Categories { get; private set; }
	private InventoryItemView _potentialItemSlot;
	
	public override void _EnterTree()
	{
		Categories = GetChildrenOfType<ICategoryTab>();
	}

	public override void _Ready()
	{
		EventBus.Instance.OnTabsUpdated += AddItemToCorrespondingTab;
		EventBus.Instance.OnInventoryItemMoved += DropInventoryItemToNewSlot; // different

		EventBus.Instance.OnSetItemSlot += SetPotentialItemSlot;
		EventBus.Instance.OnGetItemSlot += GetPotentialItemSlot;

		EventBus.Instance.OnItemClicked += SetDetailedScene;
	}
	
	public void SetInventorySizeOfTabs(int inventorySize)
	{
		foreach (var tab in Categories)
		{
			tab.SetTabInventorySize(inventorySize);
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
	
	private void AddItemToCorrespondingTab(ItemStack item)
	{
		switch (item.GetItemType())
		{
			case HerbCategory:
				Categories.OfType<HerbsTab>().FirstOrDefault()?.UpdateTabUi(item);
				break;
			case MedicineCategory:
				Categories.OfType<MedicineTab>().FirstOrDefault()?.UpdateTabUi(item);
				break;
			case SeedCategory:
				Categories.OfType<SeedsTab>().FirstOrDefault()?.UpdateTabUi(item);
				break;
		}
	}

	private void DropInventoryItemToNewSlot(ItemStack itemStack, InventoryItemView inventoryItemView)
	{
		switch (itemStack.GetItemType())
		{
			case HerbCategory:
				//Categories.OfType<HerbsTab>().FirstOrDefault()?.UpdateTabUi(item);
				break;
			case MedicineCategory:
				//Categories.OfType<MedicineTab>().FirstOrDefault()?.UpdateTabUi(item);
				break;
			case SeedCategory:
				Categories.OfType<SeedsTab>().FirstOrDefault()?.DropInventoryItemToNewSlot(itemStack, inventoryItemView.Id);
				break;
		}
	}

	private void SetPotentialItemSlot(InventoryItemView inventoryItemView)
	{
		_potentialItemSlot = inventoryItemView;
	}

	private InventoryItemView GetPotentialItemSlot()
	{
		return _potentialItemSlot;
	}

	private void SetDetailedScene(Texture2D icon, string description)
	{
		foreach (var node in _rightPanelForDetailedItemView.GetChildren())
		{
			node.QueueFree();
		}
		
		var detailedView = _detailedItemView.Instantiate<DetailedItemView>();
		detailedView.SetItemTextureRect(icon);
		detailedView.SetItemDescription(description);
		_rightPanelForDetailedItemView.AddChild(detailedView);
	}
}
