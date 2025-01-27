using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.addons.upg_utils;
using untitledplantgame.Common;
using untitledplantgame.GUI.DebugOverlays;

namespace untitledplantgame.GUI.HUDs.SettingsMenu;

public partial class DebugButton : OptionButton
{
	private LogLevel[] _logLevels;

	public override void _Ready()
	{
		// Create options
		_logLevels = Enum.GetValues<LogLevel>();
		CreateLogLevelOptions();

		// Set initial value
		if (ProjectSettings.GetSetting(DebugOverlay.DebugSettingKey, false).AsBool())
		{
			Selected = Array.IndexOf(_logLevels, Settings.Logging.GetLogLevel()) + 1; // offset because "Off" is first option
		}
		else
		{
			Selected = 0;
		}

		// Events
		ItemSelected += OnItemSelected;
	}

	private void CreateLogLevelOptions()
	{
		var index = 0;

		AddItem("Off", index);
		Selected = 0;

		foreach (var logLevel in _logLevels)
		{
			index++;
			AddItem(logLevel.ToString(), index);
		}
	}

	private void OnItemSelected(long option)
	{
		if (option == 0) // The LogLevel Enum starts with "Debug" (Debug ⇒ 0), but here, zero represents "Off"
		{
			Settings.Logging.SetLogLevel(_logLevels[^1]);

			ProjectSettings.SetSetting(DebugOverlay.DebugSettingKey, false);
			GD.Print("Disabled debug overlays.");
			return;
		}

		var level = _logLevels[option - 1];
		Settings.Logging.SetLogLevel(level);

		ProjectSettings.SetSetting(DebugOverlay.DebugSettingKey, true);
		GD.Print($"Enabled debug overlays for LogLevel: {level}");
	}
}
