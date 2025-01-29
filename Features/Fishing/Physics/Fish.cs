using Godot;

namespace untitledplantgame.Fishing.Physics;

public partial class Fish : CharacterBody2D
{
	private const float VelocityThreshold = 0.5f;
	// [Export] private Sprite2D _sprite;


	public float Speed { get; private set; }
	public Vector2 ActiveDirection { get; private set; }


	public void Initialize(float speed, Vector2 direction)
	{
		Speed = speed;
		ActiveDirection = direction.Normalized();
	}

	public override void _Process(double delta)
	{
		if (Velocity.Length() < VelocityThreshold)
		{
			ActiveDirection *= -1;
			Velocity += ActiveDirection * Speed;
		}
	}
}
