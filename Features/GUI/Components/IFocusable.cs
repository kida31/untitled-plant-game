using System;

namespace untitledplantgame.GUI.Components;

/// <summary>
///     An element that can be focused.
///     This interface is a formal reminder mostly since all Controls already implement this.
/// </summary>
public interface IFocusable
{
	public event Action FocusEntered;
	public event Action FocusExited;
}
