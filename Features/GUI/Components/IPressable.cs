using System;

namespace untitledplantgame.Inventory.GUI;

public interface IPressable
{
	public event Action Pressed;
}
