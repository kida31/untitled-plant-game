using System;

namespace untitledplantgame.GUI.Components;

/// <summary>
///		Object that has an secondary press feature.
/// </summary>
public interface ISeconaryPressable
{
	public event Action SecondaryPressed;
}
