using System;
using Godot;

namespace untitledplantgame.Fishing;

public partial class Hookbar : Area2D
{
	private untitledplantgame.Fishing.Fish _hookedFish; // Fish that is currently hooked
	public float StepDistance { get; set; }= 100; // Distance to move left or right
	public float PullDistance { get; set; }= 10;
	public float HookIsHookedDistanceMod { get; set; }= 0.25f;
	public event Action PulledFish;

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 direction;

		if (Input.IsActionJustPressed("right"))
		{
			direction = new Vector2(1, 0);
		}
		else if (Input.IsActionJustPressed("left"))
		{
			direction = new Vector2(-1, 0);
		}
		else
		{
			return;
		}

		if (_hookedFish != null)
		{
			PullFish(direction);
		}

		Move(direction);
	}

	private void PullFish(Vector2 direction)
	{
		if (_hookedFish.Direction.X * direction.X < 0)
		{
			_hookedFish.Position += direction * PullDistance;
			PulledFish?.Invoke();
		}
	}

	private void Move(Vector2 direction)
	{
		// Move the hookbar left or right
		// if moving same direction as fish, move full distance
		// if moving against fish, move 1/4 distance
		// if no fish, go full distance

		Vector2 ds;
		if (_hookedFish != null && direction.X * _hookedFish.Direction.X < 0)
		{
			ds = direction * StepDistance * HookIsHookedDistanceMod;
		}
		else
		{
			ds = direction * StepDistance;
		}

		GlobalPosition += ds;
		GD.Print("Hookbar moved");
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is not untitledplantgame.Fishing.Fish fish)
		{
			return;
		}

		_hookedFish = fish;
		fish.IsHooked = true;
	}

	private void OnAreaExited(Area2D area)
	{
		if (area is untitledplantgame.Fishing.Fish fish && fish == _hookedFish)
		{
			GD.Print("Fish unhooked");
			_hookedFish = null;
			fish.IsHooked = false;
		}
	}
}
