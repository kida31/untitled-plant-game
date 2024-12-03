using Godot;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.Inventory.PlayerInventory.UI_Buttons;
using untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

namespace untitledplantgame.Inventory.PlayerInventory;

/*
 * Reminder: C# doesn't copy objects inside of lists, but rather creates an additional reference!
 */
public partial class BookView : Control
{
	[Export] private TabsController _tabsController;
	[Export] private int _tabInventorySize;
	// TODO add custom tab buttons
	private PlayerInventory _playerInventory;

	public override void _Ready()
	{
		EventBus.Instance.OnItemPickUp += UpdateTabs;
		EventBus.Instance.OnInventoryOpen += ShowInventory;

		_tabsController.SetInventorySizeOfTabs(_tabInventorySize);

		_playerInventory = new PlayerInventory(_tabsController.Categories.Count * _tabInventorySize); //_totalInventorySize
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed(FreeRoam.OpenBook))
		{
			Visible = true;
			GameStateMachine.Instance.SetState(GameState.Book);
		} else if (@event.IsActionPressed(Book.CloseBook))
		{
			Visible = false;
			GameStateMachine.Instance.SetState(GameState.FreeRoam);
		}
	}

	private void UpdateTabs(ItemStack item)
	{
		EventBus.Instance.TabsUpdated(item);
	}

	private void ShowInventory()
	{
		Show();
	}
}
