using System;
using Godot;

namespace untitledplantgame.Player;

public partial class PixelCamera2D : Camera2D
{
	private static readonly Vector2 GameSize = new(640, 360);

	[Export] private Player _player;
	[Export(PropertyHint.Range, "0.0,5.0")] private float _speed;
	[Export(PropertyHint.Range, "0.0,1.0")] private float _mouseInfluence;
	[Export] private Vector2 _mouseBoundary = Vector2.Zero;

	private Vector2 _actualPosition;

	public override void _Ready()
	{
		_actualPosition = _player.GlobalPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var targetPosition = _player.GlobalPosition.Lerp(GetGlobalMousePosition(), _mouseInfluence);
		targetPosition = targetPosition.Clamp(
			_player.GlobalPosition - GameSize * _mouseBoundary,
			_player.GlobalPosition + GameSize * _mouseBoundary);

		_actualPosition = _actualPosition.Lerp(targetPosition, (float) Math.Clamp(_speed * delta, 0.0f, 1.0f));
		GlobalPosition = _actualPosition.Round();

		// Forward the error to the PixelViewport
		var error = GlobalPosition - _actualPosition;
		PixelViewport.Instance.SetOffset(error);
	}
}
