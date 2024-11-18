using System;
using untitledplantgame.Items;

namespace untitledplantgame.VendingMachine;

public interface IItemSlotUI
{
	ItemStack ItemStack { get; set; }
	public event Action Pressed;
}
