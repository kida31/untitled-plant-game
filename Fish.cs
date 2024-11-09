using Godot;
using System;

public partial class Fish : Area2D
{
	public float BaseSpeed { get; set; } = 200;
	public Vector2 Direction { get; private set; } = new Vector2(0, 0);
	public static float HookSpeedMod { get; set; }= 0.2f;
	public bool IsHooked { get; set; } = false;
	[Export]
	private Node2D _leftBound;
	[Export]
	private Node2D _rightBound;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// starting velocity in random direction
		Direction = new Vector2(GD.RandRange(0, 1) - 0.5f, 0f).Normalized();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var ds = Direction * BaseSpeed * (float)delta;
		// Fish moves slowly if on the hook
		if (IsHooked)
		{
			ds *= HookSpeedMod;
		}
		GlobalPosition += ds;
		// Keep fish in bounds;
		if (GlobalPosition.X < _leftBound.GlobalPosition.X || GlobalPosition.X > _rightBound.GlobalPosition.X)
		{
			Direction = -Direction;
		}
	}
}
