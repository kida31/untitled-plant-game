using System;
using Godot;
using untitledplantgame.Common;

public partial class MainMenu : Control
{
	private const string StartScene = "res://Scenes/OpeningScene.tscn";
	private const string GameScene = "res://Main.tscn";

	[Export] private TextureButton _startButton;
	[Export] private TextureButton _settingsButton;
	[Export] private TextureButton _exitButton;
	[Export] private bool _skipIntro;

	// Changed Aspect Ratio to: Keep
	// The player shouldn't be able to change that (unless we want our game to look weird)
	public override void _Ready()
	{
		_startButton.Pressed += () => { GetTree().ChangeSceneToFile(_skipIntro ? StartScene : GameScene); };
		_settingsButton.Pressed += OpenSettings;
		_exitButton.Pressed += () => GetTree().Quit();

		_startButton.GrabFocus(); // Any button would be fine.
	}

	private void OpenSettings()
	{
		GetTree().ChangeSceneToFile("res://Features/GUI/HUDs/SettingsMenu/Settings.tscn");
	}
}
