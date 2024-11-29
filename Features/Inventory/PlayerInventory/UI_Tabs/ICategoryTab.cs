using untitledplantgame.Item;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public interface ICategoryTab
{
	void SetTabInventorySize(int inventorySize);
	void UpdateTabUi(ItemStack item);
	void DropInventoryItemToNewSlot(ItemStack item, int slot);
	ItemStack[] GetItemsInCategoryTab();
}
