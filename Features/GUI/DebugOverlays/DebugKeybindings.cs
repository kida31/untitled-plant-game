using System;
using Godot;
using untitledplantgame.Common.Inputs;

namespace untitledplantgame.GUI.DebugOverlays;

/// <summary>
///     Displays the current keybindings.
/// </summary>
public partial class DebugKeybindings : Label
{
	private static readonly string[] Actions =
	{
		"base_south",
		"base_north",
		"base_east",
		"base_west",
		"base_lb",
		"base_rb",
		"base_lt",
		"base_rt",
		"base_start",
		"base_select",
	};

	public override void _Ready()
	{
		Text = CreateKeybindingsDescription();
	}

	private string CreateKeybindingsDescription()
	{
		var text = "Keybindings:\n";
		foreach (var action in Actions)
		{
			try
			{
				var button = InputRemapper.GetButton(action);
				text += $"> Gamepad {button} : ";
				var key = InputRemapper.GetKey(action);
				text += $"[{key}]\n";
			} catch(Exception e)
			{
				text += $"<Failed to find '{action}'>\n";
			}
		}

		text += "/Keybinds";
		return text;
	}
}
