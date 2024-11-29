using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class DehydratorUi : Control
{
	[Export] private Button _closeButton;
	[Export] private Button _retrieveAllItemsButton;
	[Export] private GridContainer _slotContainer;
	
	private ICraftingStation _craftingStation;

	public override void _Ready()
	{
		foreach (var VARIABLE in _craftingStation.GetAllItems())
		{
			
		}
		var Slot = new CraftingSlotUi();
		_slotContainer.AddChild(Slot);
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
	}

	private void OnCraftingStationUiItemRemoved(int slotIndex)
	{
		var item = _craftingStation.RemoveItemFromSlot(slotIndex);
		// put item in inventory
	}
}
