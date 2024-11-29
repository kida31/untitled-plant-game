using Godot;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Item;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public partial class SeedsTab : Node, ICategoryTab
{
	[Export] public string SeedsTabName;
	[Export] public string SeedsTabDescription;
	[Export] private PackedScene _inventoryItemScene;
	[Export] private PackedScene _tabItemView;

	private TabItemView _seedsItemView;
	private InventoryItemView[] _inventoryItemViews;

	public override void _EnterTree()
	{
		_seedsItemView = _tabItemView.Instantiate<TabItemView>();
		AddChild(_seedsItemView);
	}

	public override void _Process(double delta) { }

	public void SetTabInventorySize(int inventorySize)
	{
		_inventoryItemViews = new InventoryItemView[inventorySize]; // TODO: There's no need to separate this. Just let the method return an array, that easy.
		FillTabWithEmptyInventoryItemViews();
	}
	
	public void UpdateTabUi(ItemStack seedItem)
	{
		foreach (var inventoryItemView in _inventoryItemViews)
		{
			if (inventoryItemView.ItemStack == null)
			{
				inventoryItemView.UpdateItemView(seedItem);
				return;
			}
		}
	}
	
	public void DropInventoryItemToNewSlot(ItemStack item, int slot)
	{
		foreach (var inventoryItemView in _inventoryItemViews)
		{
			if (inventoryItemView.Id == slot)
			{
				// Currently no swap, only drag to new place
				inventoryItemView.UpdateItemView(item);
				return;
			}
		}
	}

	public ItemStack[] GetItemsInCategoryTab()
	{
		// Useless
		return null;
	}

	private void FillTabWithEmptyInventoryItemViews()
	{
		for (int i = 0; i < _inventoryItemViews.Length; i++)
		{
			var itemView = _inventoryItemScene.Instantiate<InventoryItemView>();
			itemView.Id = i;
			_seedsItemView.GetGridContainer().AddChild(itemView);
			_inventoryItemViews[i] = itemView;
		}
	}
}
