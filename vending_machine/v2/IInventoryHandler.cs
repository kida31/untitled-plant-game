using Godot.Collections;

namespace untitledplantgame.vending_machine;

public interface IInventoryHandler
{
    void QuickStack(IInventoryHandler target);
    void MoveAllOrDrop(IInventoryHandler target, int itemIndex);
    // Minecraft Inventory
    bool IsHotbarSlot(int i);
    // Minecraft Container
    int CountItem(IStorable item);
    int GetContainerSize();
    IItemStack GetStackInSlot(int i);
    bool IsItemValid(int n, IItemStack s);
    bool CanPlaceItem(int i, IItemStack s);
    // Bukkit interface Inventory
    IItemStack AddItem(IItemStack s);
    IItemStack RemoveItem(IItemStack s);
}

public interface IItemStack
{
    bool IsEmpty();
    bool IsSameItemSameTags(IItemStack that);
    bool IsStackable();
    int GetMaxStackSize();
    void SetCount(int n);
    int GetCount();
}