using Godot;
using System;
using Godot.NativeInterop;

public partial class lea_player : CharacterBody2D
{
	private float _moveSpeed = 100;
	private Vector2 _cardinalDirection = Vector2.Down;
	private Vector2 _direction = Vector2.Zero;
	private String _state = "idle";

	private AnimationPlayer _animationPlayer;
	private Sprite2D _sprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_sprite = GetNode<Sprite2D>("Sprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_direction.X = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		_direction.Y = Input.GetActionStrength("down") - Input.GetActionStrength("up");
		_animationPlayer.Play(_state + "_" + AnimDirection());

		Velocity = _direction * _moveSpeed;
		
		if (SetState() || SetDirection())
			UpdateAnimation();
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
		
	}

	private bool SetDirection()
	{
		Vector2 newDirection = _cardinalDirection;
		if (_direction == Vector2.Zero) 
			return false;
		
		if (_direction.Y == 0)
			if (_direction.X < 0)
				newDirection = Vector2.Left;
			else 
				newDirection = Vector2.Right;
		else if (_direction.X == 0)
			if (_direction.Y < 0)
				newDirection = Vector2.Up;
			else 
				newDirection = Vector2.Down;

		if (newDirection == _cardinalDirection)
			return false;

		_cardinalDirection = newDirection;
		
		if (_cardinalDirection == Vector2.Left)
			_sprite.Scale = new Vector2(-1, 1); //y scale is body height :)
		else _sprite.Scale = new Vector2(1, 1); //x is thicc and umfang ist auch wichtig
		
		return true;
	}

	private bool SetState()
	{
		String newState;
		if (_direction == Vector2.Zero)
			newState = "idle";
		
		else newState = "walk";

		if (newState == _state) 
			return false;
		_state = newState;
		return true;
	}

	private void UpdateAnimation()
	{
		_animationPlayer.Play(_state + "_" + AnimDirection());
	}

	private String AnimDirection()
	{
		if (_cardinalDirection == Vector2.Down)
		{
			return "down";
		}
		else if (_cardinalDirection == Vector2.Up)
		{
			return "up";
		}
		else return "side";
		
	}
}

