using System.Text.RegularExpressions;
using Godot;

namespace untitledplantgame.GUI.HUDs.SettingsMenu;

public partial class SettingsMenu : Node
{
	[Export] private Button _backButton;
	[Export] private CheckBox _checkboxFullscreen;
	[Export] private LabelForCheckButton _debugMode;
	[Export] private ResolutionButton _resolutionButton;
	[Export] private DebugButton _debugButton;

	private long _lastValidOptionButton;
	private Vector2I _gameResolution;
	
	public override void _Ready()
	{
		_backButton.Pressed += () => GetTree().ChangeSceneToFile("res://Features/GUI/HUDs/MainMenu/MainMenu.tscn");
		_resolutionButton.ItemSelected += GetSelectedItem;
		_checkboxFullscreen.Toggled += FullScreenToggled;
		_debugMode.SetDescriptiveLabelForCheckButton("ON", "OFF");
		
		SetResolutionBasedOnInitialWindowSize();
		_checkboxFullscreen.GrabFocus();
	}

	private void SetResolutionBasedOnInitialWindowSize()
	{
		for (var i = 0; i < _resolutionButton.GetItemCount(); i++)
		{
			var currentResolutionString = Regex.Replace(DisplayServer.WindowGetSize()
				.ToString(), @"[()\s]", "")
				.Replace(",", "x");

			if (currentResolutionString == _resolutionButton.GetItemText(i))
			{
				_resolutionButton.Select(i);
				_lastValidOptionButton = i;
			}
		}
	}

	private void FullScreenToggled(bool fullScreen)
	{
		if (fullScreen)
		{
			_resolutionButton.Select(-1);
		}
		else
		{
			_resolutionButton.Select((int) _lastValidOptionButton);
		}
		
		DisplayServer.WindowSetMode(fullScreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
	}

	private void GetSelectedItem(long buttonIndex)
	{
		_lastValidOptionButton = buttonIndex;
		SetGameResolutionFromOptionButtonText(buttonIndex);
		SetGameResolution();
	}

	private void SetGameResolutionFromOptionButtonText(long buttonIndex)
	{
		var resolutionString = _resolutionButton.GetItemText((int)buttonIndex); // Added just for clarification
		var parts = resolutionString.Split('x');
		
		_gameResolution = new Vector2I(int.Parse(parts[0]), int.Parse(parts[1]));
	}

	private void SetGameResolution()
	{
		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		DisplayServer.WindowSetSize(_gameResolution);
	}
}
