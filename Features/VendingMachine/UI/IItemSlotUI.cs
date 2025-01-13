using System;
using untitledplantgame.Inventory;

namespace untitledplantgame.VendingMachine;

public interface IItemSlotUI
{
	IItemStack ItemStack { get; set; }
	public event Action Pressed;
}
