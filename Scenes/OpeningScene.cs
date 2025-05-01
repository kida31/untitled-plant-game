using Godot;
using System;

public partial class OpeningScene : Control
{
	[Export] private AnimationPlayer _animationPlayer;
	public override void _Ready()
	{
		_animationPlayer.Play("OpeningScene");
		_animationPlayer.AnimationFinished += OnAnimationFinished;
	}

	private void OnAnimationFinished(StringName animName)
	{
		GetTree().ChangeSceneToFile("res://Main.tscn");
	}
}
