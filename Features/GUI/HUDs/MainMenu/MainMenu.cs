using System;
using Godot;
using untitledplantgame.Common;

public partial class MainMenu : Control
{
	[Export] private TextureButton _startButton;
	[Export] private TextureButton _settingsButton;
	[Export] private TextureButton _exitButton;
	
	// Changed Aspect Ratio to: Keep
	// The player shouldn't be able to change that (unless we want our game to look weird)
	public override void _Ready()
	{
		_startButton.Pressed += () => GetTree().ChangeSceneToFile("res://Main.tscn");
		_settingsButton.Pressed += OpenSettings;
		_exitButton.Pressed += () => GetTree().Quit();
		
		_startButton.GrabFocus();
	}
	
	private void OpenSettings()
	{
		GetTree().ChangeSceneToFile("res://Features/GUI/HUDs/SettingsMenu/Settings.tscn");
	}
}
