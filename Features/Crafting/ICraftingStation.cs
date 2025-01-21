using untitledplantgame.Inventory;
using untitledplantgame.NPC;

namespace untitledplantgame.Crafting;

/// <summary>
/// This interface defines what can be done with a crafting station.
/// </summary>
public interface ICraftingStation
{
	bool InsertItemToSlot(IItemStack item);
	IItemStack RemoveItemFromSlot(int slotIndex);
}
