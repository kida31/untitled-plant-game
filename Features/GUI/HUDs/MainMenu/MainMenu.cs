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
		
		_sfx.PlaySound("res://Assets/SFX/atmo_rain_thunder.wav");
		GetTree().ChangeSceneToFile("res://Main.tscn");
	}

	private void OpenSettings()
	{
		// Play sound here if needed
		_sfx.PlaySound("res://Assets/SFX/your_settings_sound.wav"); // Replace with actual sound path
		GetTree().ChangeSceneToFile("res://Features/GUI/HUDs/SettingsMenu/Settings.tscn");
	}

	private void OnExitButtonPressed()
	{
		// Play sound effect here if needed
		_sfx.PlaySound("res://Assets/SFX/your_exit_sound.wav"); // Replace with actual sound path
		GetTree().Quit();
	}
}
