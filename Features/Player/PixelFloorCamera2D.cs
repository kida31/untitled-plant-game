using System;
using Godot;

namespace untitledplantgame.Player;

public partial class PixelFloorCamera2D : Camera2D
{
	[Export] private Player _player;
	[Export(PropertyHint.ExpEasing)] private float _smoothing = Int32.MaxValue;
	[Export(PropertyHint.Range, "0.0,1.0")] private float _mouseInfluence;
	[Export] private Vector2 _mouseBoundary = Vector2.Zero;

	private Vector2 _gameSize = new(640, 360);

	private float _windowScale = 1.0f;

	private Vector2 _actualPosition;

	public override void _Ready()
	{
		_windowScale = (GetWindow().Size / _gameSize).X;
		_actualPosition = GlobalPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var targetPosition = _player.GlobalPosition.Lerp(GetGlobalMousePosition(), _mouseInfluence);
		targetPosition = targetPosition.Clamp(
			_player.GlobalPosition - _gameSize * _mouseBoundary,
			_player.GlobalPosition + _gameSize * _mouseBoundary
		);

		_actualPosition = _actualPosition.Lerp(targetPosition, (float) delta * 100 / Math.Max(1, _smoothing));

		var error = _actualPosition.Round() - _actualPosition;
		PixelViewport.Instance.SetOffset(error);
		GlobalPosition = _actualPosition.Round();
	}
}
