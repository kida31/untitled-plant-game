using Godot;
using System.Collections.Generic;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public partial class PlayerInventoryPage : HBoxContainer
{
	[Export] private TabsController _inventoryTabs;
	[Export] private PlayerDetailsView _playerDetails;

	public void SetInventories(List<Inventory> inventories)
	{
		_inventoryTabs.SetInventories(inventories);
	}

	public void SetPlayerDetails(object obj)
	{
		_playerDetails.SetPlayerDetails(obj);
	}

	public override void _GuiInput(InputEvent @event)
	{
		GD.Print(@event);
	}
}
