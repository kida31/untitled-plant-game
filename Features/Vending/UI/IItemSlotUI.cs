using System;
using untitledplantgame.Inventory;

namespace untitledplantgame.Vending.UI;

public interface IItemSlotUI
{
	IItemStack ItemStack { get; set; }
	public event Action Pressed;
}
