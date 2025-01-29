
using Godot;
using System;
using untitledplantgame.Common.Inputs.GameActions;

namespace untitledplantgame.Fishing.New;
public partial class FishingRod : Area2D
{
    [Export] private Label _debugLabel;
	[Export] private CollisionShape2D _collisionShape;

    public float Speed;
    public float SpeedAgainstFish;

    private Fish _attachedFish;
	private CircleShape2D _circleShape;

	public Fish Fish => _attachedFish;
    public Vector2 ActiveDirection { get; private set; } = Vector2.Zero;
    public Vector2 Velocity { get; private set; } = Vector2.Zero;

    public override void _Ready()
    {
        AreaEntered += (area) => OnAreaEntered(area);
        AreaExited += (area) => OnAreaExited(area);
		_circleShape = _collisionShape.Shape as CircleShape2D;
		if (_circleShape == null) {
			GD.PushError("Shape should be a circle");
			QueueFree();
		}
    }

	public void Initialize(float width, float speed, float speedOpposite)
	{
		_circleShape.Radius = width / 2f;
		Speed = speed;
		SpeedAgainstFish = speedOpposite;
	}

	private void OnAreaExited(Area2D area)
	{
        // Detach fish
		if (area is Fish fish && _attachedFish == fish) {
            _attachedFish = null;
        }
	}

	private void OnAreaEntered(Area2D area)
	{
        // Attach fish
		if (area is Fish fish) {
            _attachedFish ??= fish;
        }
	}

	public override void _Process(double delta)
	{
		// Process Input
        var inputHorizontal = Vector2.Zero;
        inputHorizontal.X = Input.GetAxis(Base.Left, Base.Right);
        ActiveDirection = inputHorizontal;

		// Update velocity
		if (ActiveDirection == Vector2.Zero)
		{
			// No input
			if (_attachedFish != null)
			{
				Velocity = _attachedFish.Velocity;
                _debugLabel.Text = "Dragged";
				return;
			}
			else
			{
				Velocity = Vector2.Zero;
                _debugLabel.Text = "Idle";
				return;
			}
		}
		else
		{
			// Has Input
			if (_attachedFish == null)
			{
				// Input and no fish => full control
				var vel = ActiveDirection * Speed;
				vel.Y = 0;
				Velocity = vel;
                _debugLabel.Text = "Free";
				return;
			}
			else
			{
				// Input and fish attached => full speed if same direction, reduced if against current
				if (ActiveDirection.X * _attachedFish.ActiveDirection.X > 0)
				{
					// same direction
					var vel = ActiveDirection * Speed;
					vel.Y = 0;
					Velocity = vel;
                    _debugLabel.Text = "Following";
					return;
				}
				else
				{
					// opposite direction
					var vel = ActiveDirection * SpeedAgainstFish;
					vel.Y = 0;
					Velocity = vel;
                    _debugLabel.Text = "Pulling";
					return;
				}
			}
		}
	}

	public override void _PhysicsProcess(double delta)
    {
        Position += (float)delta * Velocity;
    }
}

