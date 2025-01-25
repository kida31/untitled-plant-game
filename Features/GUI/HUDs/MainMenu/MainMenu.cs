using System;
using Godot;
using untitledplantgame.Audio;

public partial class MainMenu : Control
{
	[Export] private Button _startButton;
	[Export] private Button _settingsButton;
	[Export] private Button _exitButton;

	private SfxUI _sfx;

	public override void _Ready()
	{
		_sfx = new SfxUI();
		_sfx._Ready();
		GetTree().Root.AddChild(_sfx); // Add SfxUI to the root node
		_sfx.Name = "SfxUI"; // Optional: Name the node for easier debugging

		_startButton.Pressed += OnStartButtonPressed;
		_settingsButton.Pressed += OpenSettings;
		_exitButton.Pressed += OnExitButtonPressed;
	}

	private void OnStartButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Main.tscn");
	}

	private void OpenSettings()
	{
		GetTree().ChangeSceneToFile("res://Features/GUI/HUDs/SettingsMenu/Settings.tscn");
	}

	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
