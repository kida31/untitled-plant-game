using Godot;
using untitledplantgame.Inventory;
using untitledplantgame.VendingMachine;

namespace untitledplantgame.Crafting;

public partial class DehydratorUi : Control
{
	[Export] private Button _closeButton;
	[Export] private Button _retrieveAllItemsButton;
	[Export] private GridContainer _slotContainer;
	
	private Dehydrator _craftingStation;

	public override void _Ready()
	{
		_craftingStation = GetNode<Dehydrator>("/root/Game/Crafting/Dehydrator");
		
		foreach (var itemStack in _craftingStation.GetAllItems())
		{
			var slot = new ItemSlotUI();
			slot.ItemStack = itemStack;
			
			_slotContainer.AddChild(slot);
		}
	}

	private void OnCraftingStationUiOpened()
	{
		Visible = true;
	}

	private void OnCraftingStationUiClosed()
	{
		Visible = false;
	}

	private void OnCraftingStationUiItemInserted(ItemStack item, int slotIndex)
	{
		_craftingStation.InsertItemToSlot(item, slotIndex);
		if (_slotContainer.GetChild(slotIndex) is ItemSlotUI slot)
		{
			slot.ItemStack = item;
		}
		// remove item from inventory
	}

	private void OnCraftingStationUiItemRemoved(int slotIndex)
	{
		var item = _craftingStation.RemoveItemFromSlot(slotIndex);
		if (_slotContainer.GetChild(slotIndex) is ItemSlotUI slot)
		{
			slot.ItemStack = null;
		}
		// put item in inventory
	}
}
