using System;
using Godot;

namespace untitledplantgame.Player;

[Obsolete("Disabling smoothing in built-in camera seems to be enough")]
public partial class PixelCamera2D : Camera2D
{
	[Export] private bool _smoothingEnabled = true;
	[Export] private int _smoothingFactor = 8;
	[Export] private int _stepsPx = 1;

	private Vector2 _initialOffset;
	private Node2D _parent;

	public override void _Ready()
	{
		TopLevel = true;
		_parent = GetParent<Node2D>();
		_initialOffset = Vector2.Zero;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_smoothingEnabled)
		{
			GlobalPosition = _parent.GlobalPosition + _initialOffset;
			return;
		}

		var weight = (11f - _smoothingFactor) / 100f;
		GlobalPosition = GlobalPosition.Lerp(_parent.GlobalPosition + _initialOffset, weight);

		GlobalPosition = new Vector2(
			Mathf.Floor(GlobalPosition.X / _stepsPx) * _stepsPx,
			Mathf.Floor(GlobalPosition.Y / _stepsPx) * _stepsPx
		);
		var other = (GlobalPosition / _stepsPx).Floor() * _stepsPx;
		GD.Print(_parent.Name, " ", GlobalPosition, " samesame ", other, " :: ", other == GlobalPosition );
	}
}
