using Godot;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Item;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public partial class MedicineTab : Node, ICategoryTab
{
	[Export] public string MedicineTabName;
	[Export] public string MedicineTabDescription;
	[Export] private PackedScene _inventoryItemScene;
	[Export] private PackedScene _tabItemView;

	private TabItemView _medicineItemView;
	private ItemStack[] _itemStacks;
	private InventoryItemView[] _inventoryItemViews;

	public override void _EnterTree()
	{
		_medicineItemView = _tabItemView.Instantiate<TabItemView>();
		AddChild(_medicineItemView);
	}

	public override void _Process(double delta) { }

	public void SetTabInventorySize(int inventorySize)
	{
		_itemStacks = new ItemStack[inventorySize];
		_inventoryItemViews = new InventoryItemView[inventorySize];
		
		FillTabWithEmptyInventoryItemViews();
	}
	
	public void UpdateTabUi(ItemStack seedItem)
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
	
	public void DropInventoryItemToNewSlot(ItemStack item, int slot)
	{
		
	}

	public ItemStack[] GetItemsInCategoryTab()
	{
		return _itemStacks;
	}

	private void FillTabWithEmptyInventoryItemViews()
	{
		for (int i = 0; i < _inventoryItemViews.Length; i++)
		{
			var itemView = _inventoryItemScene.Instantiate<InventoryItemView>();

			_medicineItemView.GetGridContainer().AddChild(itemView);
			_inventoryItemViews[i] = itemView;
		}
	}
}
