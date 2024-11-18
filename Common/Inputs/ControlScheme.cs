using System.Collections.Generic;
using System.Linq;
using Godot;

namespace untitledplantgame.Common.Inputs;

/// <summary>
/// Class to to get the key or button for a given action name.
/// </summary>
internal abstract class ControlScheme
{
	private static readonly Dictionary<string, Key> _keys = new();
	private static readonly Dictionary<string, JoyButton> _buttons = new();

	/// <summary>
	/// Returns they Key that is mapped to this action
	/// </summary>
	/// <param name="actionName"></param>
	/// <returns></returns>
	internal static Key GetKey(string actionName)
	{
		// freeroam_interact => base_interact
		actionName = InputRemapper.GetBaseAction(actionName);

		if (!_keys.ContainsKey(actionName))
		{
			var inputEvent = InputMap.ActionGetEvents(actionName).First(e => e is InputEventKey) as InputEventKey;
			_keys[actionName] = inputEvent!.PhysicalKeycode;
		}

		return _keys[actionName];
	}

	/// <summary>
	/// Returns they gamepad button that is mapped to this action
	/// </summary>
	/// <param name="actionName"></param>
	/// <returns></returns>
	internal static JoyButton GetButton(string actionName)
	{
		actionName = InputRemapper.GetBaseAction(actionName);

		if (!_buttons.ContainsKey(actionName))
		{
			var inputEvent = (InputEventJoypadButton)InputMap.ActionGetEvents(actionName).First(e => e is InputEventJoypadButton);
			_buttons[actionName] = inputEvent.ButtonIndex;
		}

		return _buttons[actionName];
	}
}
