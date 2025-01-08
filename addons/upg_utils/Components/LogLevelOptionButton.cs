using System;
using System.Linq;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.addons.upg_utils.Components;

[Tool]
public partial class LogLevelOptionButton : OptionButton
{
	public override void _Ready()
	{
		VisibilityChanged += () =>
		{
			if (IsVisibleInTree()) OnVisible();
		};

		ItemSelected += index =>
		{
			if (index < 0) return;
			var levels = Enum.GetValues<LogLevel>();
			var level = levels.ElementAt((int) index);
			Settings.Logging.SetLogLevel(level);
		};
	}

	private void OnVisible()
	{
		// Reload content
		GD.Print("Reloading LogLevelOptionButton content.");
		Clear();

		var levels = Enum.GetValues<LogLevel>();
		foreach (var level in levels)
		{
			AddItem(level.ToString());
		}

		// Set current value
		var logLevel = Settings.Logging.GetLogLevel();
		if (logLevel.HasValue)
		{
			var index = levels.ToList().IndexOf(logLevel.Value);
			Select(index);
		}
		else
		{
			Select(-1);
		}
	}
}
