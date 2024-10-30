
using Godot.Collections;
using untitledplantgame.EntityStatsDataContainer;

namespace untitledplantgame.Inventory.GeneralInventory.UI_Tabs;

public interface ICategoryTab
{
    void UpdateItemsInTab(DataContainer herbItem);
    Array<DataContainer> GetItemsInCategoryTab();
}
