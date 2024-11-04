using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Player;

public partial class Player : CharacterBody2D
{
	[Export]
	private InteractablesManager _interactablesManager;

	private readonly Logger _logger = new Logger("Player");
	private Vector2 _cardinalDirection = Vector2.Down;
	public Vector2 Direction = Vector2.Zero;

	private AnimatedSprite2D _animatedSprite2D;
	private PlayerStateMachine _stateMachine;

	public override void _Ready()
	{
		_logger.Info("! Ready !");
		_stateMachine = GetNode<PlayerStateMachine>("StateMachine");
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_stateMachine.Initialize(this);
	}

	public override void _Process(double delta)
	{
		Direction.X = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		Direction.Y = Input.GetActionStrength("down") - Input.GetActionStrength("up");

		//Velocity = direction * MoveSpeed;
		_interactablesManager.ScanForInteractables();
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public bool SetDirection()
	{
		Vector2 newDirection = _cardinalDirection;

		if (Direction == Vector2.Zero)
			return false;

		if (Direction.Y == 0)
		{
			if (Direction.X < 0)
				newDirection = Vector2.Left;
			else
				newDirection = Vector2.Right;
		}

		if (Direction.X == 0)
		{
			if (Direction.Y > 0)
				newDirection = Vector2.Down;
			else
				newDirection = Vector2.Up;
		}

		_cardinalDirection = newDirection;
		return true;
	}

	public void UpdateAnimation(string state)
	{
		var animationState = state + "_" + AnimationDirection();
		_animatedSprite2D.Play(animationState);
	}

	string AnimationDirection()
	{
		if (_cardinalDirection == Vector2.Down)
			return "down";
		if (_cardinalDirection == Vector2.Up)
			return "up";
		if (_cardinalDirection == Vector2.Left)
			return "left";
		if (_cardinalDirection == Vector2.Right)
			return "right";

		throw new InvalidOperationException("Invalid direction; Unreachable Code");
	}
}
