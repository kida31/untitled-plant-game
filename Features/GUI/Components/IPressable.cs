using System;

namespace untitledplantgame.GUI.Components;

/// <summary>
///     An element that can be pressed.
/// </summary>
public interface IPressable
{
	public event Action Pressed;
}
