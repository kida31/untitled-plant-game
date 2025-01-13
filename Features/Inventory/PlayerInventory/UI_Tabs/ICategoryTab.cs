using untitledplantgame.Item;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public interface ICategoryTab
{
	void SetTabInventorySize(int inventorySize);
	void UpdateTabUi(IItemStack item);
	void DropInventoryItemToNewSlot(IItemStack item, int slot);
}
