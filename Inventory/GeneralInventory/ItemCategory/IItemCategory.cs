using Godot;
using System;
using System.Xml;

namespace untitledplantgame.Inventory.GeneralInventory;

public interface IItemCategory
{
    string GetIItemCategoryName();

    IItemCategory GetIItemCategory();

}
