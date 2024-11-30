using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

/// <summary>
/// This interface defines what can be done with a crafting station.
/// </summary>
public interface ICraftingStation
{
	void InsertItemToSlot(ItemStack item, int slotIndex);
	ItemStack RemoveItemFromSlot(int slotIndex);
}
