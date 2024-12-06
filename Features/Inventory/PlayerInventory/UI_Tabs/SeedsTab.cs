using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public partial class SeedsTab : Node, ICategoryTab
{
	[Export] public string SeedsTabName;
	[Export] public string SeedsTabDescription;
	[Export] private PackedScene _inventoryItemScene;
	[Export] private PackedScene _tabItemView;
	[Export] private InventoryItemView _moving;
	
	private TabItemView _seedsItemView;
	private InventoryItemView[] _inventoryItemViews;

	public override void _EnterTree()
	{
		_seedsItemView = _tabItemView.Instantiate<TabItemView>();
		AddChild(_seedsItemView);
	}

	public override void _Ready()
	{
		EventBus.Instance.OnInventoryItemViewPressed += CreateHoverItemView;
		EventBus.Instance.OnInventoryItemViewMoved += MoveHoverItemView;
		EventBus.Instance.OnInventoryItemViewReleased += DisabledHoverItemView;
		
		// Temporary Solution to highlight first element
		EventBus.Instance.OnInventoryOpen += HighlightFirstElementOfSeedTab;
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

	// Delete after MVP
	private void HighlightFirstElementOfSeedTab()
	{
		foreach (var inventoryItemView in _inventoryItemViews)
		{
			if (inventoryItemView.ItemStack != null)
			{
				inventoryItemView.SetThisItemViewFocused();
			}
		}
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

	private void CreateHoverItemView(InventoryItemView inventoryItemView)
	{
		if (inventoryItemView.ItemStack != null && _moving.ItemStack == null)
		{
			_moving.Show();

			var slightlyOffPosition = new Vector2(20, -20);
			slightlyOffPosition += inventoryItemView.GlobalPosition;
			_moving.GlobalPosition = slightlyOffPosition;
			_moving.UpdateItemView(inventoryItemView.ItemStack);
			inventoryItemView.UpdateItemView(null);
		}
	}

	private void MoveHoverItemView(InventoryItemView inventoryItemView)
	{
		var slightlyOffPosition = new Vector2(20, -20);
		slightlyOffPosition += inventoryItemView.GlobalPosition;
		_moving.GlobalPosition = slightlyOffPosition;
	}

	private void DisabledHoverItemView(InventoryItemView inventoryItemView)
	{
		if (inventoryItemView.ItemStack == null)
		{
			inventoryItemView.UpdateItemView(_moving.ItemStack);
			_moving.UpdateItemView(null);
			_moving.Hide();
		}
	}
	
	// Delete after MVP
	private void SafeTanglingItemView()
	{
		foreach (var inventoryItemView in _inventoryItemViews)
		{
			if (inventoryItemView.ItemStack != null)
			{
				inventoryItemView.UpdateItemView(_moving.ItemStack);
				inventoryItemView.SetThisItemViewFocused();
				_moving.UpdateItemView(null);
				_moving.Hide();
				return;
			}
		}
	}
}
