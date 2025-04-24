using Godot;
using System;

public partial class IntroCutscene : VideoStreamPlayer
{
	private const string GameScene = "res://Main.tscn";

	public override void _Ready()
	{
		Finished += OnCutsceneFinished;
	}

	private void OnCutsceneFinished()
	{
		GetTree().ChangeSceneToFile(GameScene);
	}
}
