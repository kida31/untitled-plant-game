using untitledplantgame.Item;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public interface ICategoryTab
{
	void SetTabInventorySize(int inventorySize);
	void UpdateTabUi(untitledplantgame.Item.ItemStack item);
	void DropInventoryItemToNewSlot(untitledplantgame.Item.ItemStack item, int slot);
	untitledplantgame.Item.ItemStack[] GetItemsInCategoryTab();
}
