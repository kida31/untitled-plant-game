﻿using System;
using untitledplantgame.Inventory;

namespace untitledplantgame.VendingMachine;

public interface IItemSlotUI
{
	ItemStack ItemStack { get; set; }
	public event Action Pressed;
}
