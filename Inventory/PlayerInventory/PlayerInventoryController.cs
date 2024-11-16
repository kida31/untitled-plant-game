using Godot;
using untitledplantgame.Inventory.PlayerInventory.UI_Tabs;
using untitledplantgame.Event;


namespace untitledplantgame.Inventory.PlayerInventory;

/*
 * Reminder: C# doesn't copy objects inside of lists, but rather creates an additional reference!
 */
public partial class PlayerInventoryController : Node
{
	[Export] private TabsController _tabsController;
	[Export] private int _tabInventorySize;
	
	private PlayerInventory _playerInventory;

	public override void _Ready()
	{
		EventBus.Instance.OnItemPickUp += UpdateTabs;
		
		_tabsController.SetInventorySizeOfTabs(_tabInventorySize);
		
		_playerInventory = new PlayerInventory(_tabsController.Categories.Count * _tabInventorySize); //_totalInventorySize
	}
	
	public override void _Process(double delta) { }

	private void UpdateTabs(ItemStack item)
	{
		EventBus.Instance.TabsUpdated(item);
	}
}
