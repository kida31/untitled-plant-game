using System;

namespace untitledplantgame.Inventory.GUI;

public interface IFocusable
{
	public event Action FocusEntered;
	public event Action FocusExited;
}
