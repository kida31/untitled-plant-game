using Godot;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Item;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public partial class HerbsTab : Node, ICategoryTab
{
	[Export] public string HerbsTabName;
	[Export] public string HerbsTabDescription;
	[Export] private PackedScene _inventoryItemScene;
	[Export] private PackedScene _tabItemView;
	
	private TabItemView _herbsItemView;
	private IItemStack[] _itemStacks;
	private InventoryItemView[] _inventoryItemViews;

	public override void _EnterTree()
	{
		_herbsItemView = _tabItemView.Instantiate<TabItemView>();
		AddChild(_herbsItemView);
	}

	public override void _Process(double delta) { }

	public void SetTabInventorySize(int inventorySize)
	{
		_itemStacks = new ItemStack[inventorySize];
		_inventoryItemViews = new InventoryItemView[inventorySize];
		
		FillTabWithEmptyInventoryItemViews();
	}
	
	public void UpdateTabUi(IItemStack seedItem)
	{
		for (var i = 0; i < _itemStacks.Length; i++)
		{
			if (_itemStacks[i] == null)
			{
				_itemStacks[i] = seedItem;
				return;
			}
		}
	}
	
	public void DropInventoryItemToNewSlot(IItemStack item, int slot)
	{
		
	}

	public IItemStack[] GetItemsInCategoryTab()
	{
		return _itemStacks;
	}

	private void FillTabWithEmptyInventoryItemViews()
	{
		for (int i = 0; i < _inventoryItemViews.Length; i++)
		{
			var itemView = _inventoryItemScene.Instantiate<InventoryItemView>();

			_herbsItemView.GetGridContainer().AddChild(itemView);
			_inventoryItemViews[i] = itemView;
		}
	}
}
