using untitledplantgame.Inventory;

/// <summary>
/// This interface describes the methods needed to interact with a crafting station UI.
/// </summary>
public interface ICraftingStationUi
{
	void OnCraftingStationUiOpened();
	void OnCraftingStationUiClosed();
	void OnCraftingStationUiItemInserted(ItemStack item, int slotIndex);
	void OnCraftingStationUiItemRemoved(ItemStack item, int slotIndex);
}
