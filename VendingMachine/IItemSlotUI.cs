using System;
using untitledplantgame.Inventory.Alt;

namespace GUI.VendingMachine;

public interface IItemSlotUI
{
	ItemStack ItemStack { get; set; }
	public event Action Pressed;
}
