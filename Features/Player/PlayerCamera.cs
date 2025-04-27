using Godot;
using System;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Player;

public partial class PlayerCamera : Camera2D
{
	private Player _player;
	private Vector2 _previousPosition;
	private float _speed = 5f;
	private Logger _logger = new("PlayerCamera");

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
		_logger.Debug("Panning to position: " + targetPosition);
		var vp = GetViewport();
		vp.SetDisableInput(true);
		vp.GuiDisableInput = true;
		vp.SetInputAsHandled(); 
		
		_previousPosition = GlobalPosition;
		var tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Sine);
		tween.SetEase(Tween.EaseType.Out);
		
		tween.TweenProperty(this, "global_position", targetPosition, 1.0f);
		tween.TweenProperty(this, "global_position", _player.GlobalPosition, 1.0f)
			.SetDelay(1.0f); // wait 1 second before returning;
		
		ToSignal(tween, Tween.SignalName.Finished).OnCompleted(() =>
		{
			_logger.Debug("Returning to previous position" + _previousPosition);
			vp.SetDisableInput(false);
			vp.GuiDisableInput = false;
			vp.SetInputAsHandled();
		});
	}
}
