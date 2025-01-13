using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

/// <summary>
/// This interface defines what can be done with a crafting station.
/// </summary>
public interface ICraftingStation
{
	void InsertItemToSlot(IItemStack item, int slotIndex);
	IItemStack RemoveItemFromSlot(int slotIndex);
}
