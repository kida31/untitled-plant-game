using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class DehydratorUi : Control
{
	[Export] private Button _closeButton;
	[Export] private Button _retrieveAllItemsButton;
	
	private ICraftingStation _craftingStation;

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
	}

	private void OnCraftingStationUiItemRemoved(ItemStack item, int slotIndex)
	{
	}
}
