using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.Inventory;
using untitledplantgame.Shops;
using untitledplantgame.Tools;

namespace untitledplantgame.Player;

public partial class Player : CharacterBody2D
{
	private readonly Logger _logger = new ("Player");

	// Input direction(?)
	public Vector2 Direction = Vector2.Zero;

	/// <summary>
	/// The direction the player is facing.
	/// </summary>
	public Vector2 FrontDirection => _frontDirection;

	public BigInventory Inventory => _inventory;
	public Toolbelt Toolbelt => _toolbelt;

	private Vector2 _cardinalDirection = Vector2.Down;
	private Vector2 _frontDirection = Vector2.Down; // The direction the player is facing.
	private AnimatedSprite2D _animatedSprite2D;
	private PlayerStateMachine _stateMachine;
	private BigInventory _inventory;

	private readonly Toolbelt _toolbelt = new(
		new Tool[]
		{
			new Shears(12, 16),
			new WateringCan(50, 1000, true, 12, 24),
			new SeedBag(12, 24, 1f),
			new Shovel(12, 16, 1.5f)
		}
	);

	public override void _Ready()
	{
		_logger.Info("! Ready !");

		Game.Instance.Provide(this);

		_stateMachine = GetNode<PlayerStateMachine>("StateMachine");
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_stateMachine.Initialize(this);

		EventBus.Instance.OnItemPickUp += OnItemPickUp;

		// Initialize inventory
		var rand = new RandomStockGenerator();
		_inventory = new(20);
		_inventory.InventoryChanged += () => { EventBus.Instance.PlayerInventoryChanged(this, _inventory); };
		// _inventory.AddItem(rand.GetRandomItems(12).ToArray()); // For testing purposes
	}

	private void OnItemPickUp(IItemStack obj)
	{
		if (obj == null)
		{
			_logger.Error("Item is null, cannot pick up.");
			return;
		}

		var leftovers = _inventory.AddItem(obj);
		if (leftovers.Count > 0)
		{
			_logger.Warn("Inventory full, could not pick up all items. This is not handled");
		}
	}

	private Player InitializePlayer()
	{
		return this;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (GameStateMachine.Instance.CurrentState != GameState.FreeRoam)
		{
			Direction = Vector2.Zero; // default value, movement is an exception
		}
	}

	public void GetSetInputDirection()
	{
		Direction = Input.GetVector(FreeRoam.Left, FreeRoam.Right, FreeRoam.Up, FreeRoam.Down).Normalized();
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
		UpdateFrontDirection();
	}

	public bool SetDirection()
	{
		Vector2 newDirection = _cardinalDirection;

		if (Direction == Vector2.Zero)
			return false;

		if (Direction.Y == 0)
		{
			if (Direction.X < 0)
			{
				newDirection = Vector2.Left;
				_animatedSprite2D.FlipH = true;
			}

			else
			{
				newDirection = Vector2.Right;
				_animatedSprite2D.FlipH = false;
			}
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

	/// <summary>
	/// Update front direction (the direction the player is facing).
	/// The front direction equals the latest input direction. "Clamping" to NWSE (non-diagonal).
	/// Horizontal directions have priority. If horizontal direction is zero, vertical direction is chosen.
	/// If there is no input direction, keep latest front direction value.
	/// </summary>
	private void UpdateFrontDirection()
	{
		if (Direction == Vector2.Zero)
		{
			return;
		}

		_frontDirection = Direction;
		if (Direction.X != 0)
		{
			_frontDirection.Y = 0;
		}
		else
		{
			_frontDirection.X = 0;
		}

		_frontDirection = _frontDirection.Normalized();
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
