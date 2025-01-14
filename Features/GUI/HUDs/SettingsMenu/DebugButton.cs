using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.addons.upg_utils;
using untitledplantgame.Common;

namespace untitledplantgame.GUI.HUDs.SettingsMenu;
public partial class DebugButton : OptionButton
{
	private List<LogLevel> _logLevels;
	
	public override void _Ready()
	{
		ItemSelected += ChangeLogLevel;
		
		_logLevels = Enum.GetValues<LogLevel>().ToList();
		CreateLogLevelOptions();
	}

	private void CreateLogLevelOptions()
	{
		var index = 0;

		AddItem("None", index);

		foreach (var logLevel in _logLevels)
		{
			index++;
			AddItem(logLevel.ToString(), index);
		}
	}

	private void ChangeLogLevel(long selectedLogLevel)
	{
		if (selectedLogLevel - 1 == -1) // The LogLevel Enum starts with "Debug" (Debug ⇒ 0), but here, zero represents "None"
		{
			EventBus.DisplayLog = false;
			return;
		}
		
		Settings.Logging.SetLogLevel(Enum.GetValues<LogLevel>()[selectedLogLevel-1]);
		EventBus.DisplayLog = true;
		GD.PrintRich(Settings.Logging.GetLogLevel());
	}
}
