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
		VisibilityChanged += OnVisibilityChanged;
		ItemSelected += OnItemSelected;
		ProjectSettings.SettingsChanged += UpdateItems;
	}
	
	public override void _ExitTree()
	{
		VisibilityChanged -= OnVisibilityChanged;
		ItemSelected -= OnItemSelected;
		ProjectSettings.SettingsChanged -= UpdateItems;
	}

	private void UpdateItems()
	{
		Clear();

		// Add log levels
		var levels = Enum.GetValues<LogLevel>().ToList();
		levels.ForEach(l => AddItem(l.ToString()));

		// Set current value
		var logLevel = Settings.Logging.GetLogLevel();
		Select(logLevel.HasValue ? levels.IndexOf(logLevel.Value) : -1);
	}

	private void OnItemSelected(long index)
	{
		if (index < 0) return;
		var levels = Enum.GetValues<LogLevel>();
		var level = levels.ElementAt((int) index);
		Settings.Logging.SetLogLevel(level);
	}

	private void OnVisibilityChanged()
	{
		if (IsVisibleInTree()) UpdateItems();
	}
}
