using System;

namespace untitledplantgame.Inventory.GUI;

public interface IPressable
{
	public event Action Pressed;
}

public interface IFocusable
{
	public event Action FocusEntered;
	public event Action FocusExited;
}

public interface INavigatable: IPressable, IFocusable
{
	
}
