using Godot;
using System;
using untitledplantgame.Common;
using untitledplantgame.Player;

public partial class PlayerCamera : Camera2D
{
	private Player _player;
	private Vector2 _previousPosition;
	private float _speed = 5f;

	public override void _Ready()
	{
		EventBus.Instance.OnCameraMoveAndBack += PanToPosition;
		_player = GetParent<Player>();
	}

	private void ReturnToPreviousPosition()
	{
		GlobalPosition = _player.GlobalPosition;
	}

	private void PanToPosition(Vector2 targetPosition)
	{
		_previousPosition = GlobalPosition;
		var tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Sine);
		tween.SetEase(Tween.EaseType.Out);
		tween.TweenProperty(this, "global_position", targetPosition, 1.0f);
		tween.TweenProperty(this, "global_position", _previousPosition, 1.0f)
			.SetDelay(1.0f); // wait 1 second before returning;
	}
}
