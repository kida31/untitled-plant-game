using System;
using Godot;

namespace untitledplantgame.Fishing.Classic;

/// <summary>
///		This class represents a fish that can be caught by the player.
///		It simply moves forward towards its ActiveDirection.
/// </summary>
public partial class Fish : Area2D
{
	[Export] private Sprite2D _sprite;
	private FishingRod _rod;

	public float Speed { get; private set; }
	public float SpeedOppositeHook { get; private set; }

	public Vector2 ActiveDirection { get; private set; }

	[Export]
	public Vector2 Velocity { get; private set; }

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;
	}

	public void Initialize(float speed, float speedOpposite, Vector2 direction)
	{
		Speed = speed;
		SpeedOppositeHook = speedOpposite;
		ActiveDirection = direction.Normalized();
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is FishingRod rod)
		{
			_rod ??= rod;
		}
	}
	private void OnAreaExited(Area2D area)
	{
		if (area is FishingRod rod && _rod == rod)
		{
			_rod = null;
		}
	}

	public override void _Process(double delta)
	{
		_sprite.FlipH = ActiveDirection.X > 0;

		if (_rod == null || _rod.Velocity == Vector2.Zero)
		{
			Velocity = ActiveDirection * Speed;
			return;
		}
		else
		{
			// _rod.Velocity > 0
			if (_rod.ActiveDirection.X * ActiveDirection.X >= 0)
			{
				// Same direction
				Velocity = ActiveDirection * Speed;
				return;
			}
			else
			{
				// Opposite direction
				Velocity = _rod.ActiveDirection.Normalized() * SpeedOppositeHook;
				return;
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += (float)delta * Velocity;
	}
}
