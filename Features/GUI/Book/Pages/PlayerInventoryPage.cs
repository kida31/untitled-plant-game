using System.Collections.Generic;
using Godot;
using untitledplantgame.GUI.Book.Inventories;

namespace untitledplantgame.Inventory.PlayerInventory;

public partial class PlayerInventoryPage : Control
{
	[Export] private InventoryTabsView _inventoryTabs;
	[Export] private PlayerDetailsView _playerDetails;

	public override void _Ready()
	{
		VisibilityChanged += OnVisibilityChanged;
	}

	private void OnVisibilityChanged()
	{
		if (IsVisibleInTree())
		{
			_inventoryTabs.GrabFocus();
		}
	}

	public void UpdateInventories(List<IInventory> inventories)
	{
		_inventoryTabs.UpdateInventories(inventories);
	}

	public void UpdatePlayerDetails(object obj)
	{
		_playerDetails.SetPlayerDetails(obj);
	}
}
