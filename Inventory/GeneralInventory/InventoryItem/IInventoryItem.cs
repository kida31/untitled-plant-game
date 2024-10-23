using untitledplantgame.Inventory.GeneralInventory;

public interface IInventoryItem
{
    string GetInventoryItemName();

    IItemCategory GetItemCategoryName();
}
