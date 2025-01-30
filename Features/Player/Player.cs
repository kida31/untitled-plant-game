using System;
using System.Linq;
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
	[Export] private PlayerAnimatedSprite _playerAnimatedSprite;

	private readonly Logger _logger = new("Player");

	private Vector2 _faceDirection = Vector2.Down;
	private Vector2 _inputDirection = Vector2.Zero;
	private PlayerStateMachine _stateMachine;

	/// <summary>
	///     The direction the player should be moving. Currently always equal to input direction.
	/// </summary>
	public Vector2 Direction => _inputDirection;

	/// <summary>
	///     The direction the player is facing. This is the direction the player is looking at. Visually
	/// </summary>
	public Vector2 FaceDirection => _faceDirection;

	public BigInventory Inventory { get; private set; }

	public Toolbelt Toolbelt { get; } = new(
		new Tool[]
		{
			// Hardcoded tools for now
			GD.Load<Tool>("res://Resources/Tools/WateringCan.tres"),
			GD.Load<Tool>("res://Resources/Tools/SeedBag.tres"),
			GD.Load<Tool>("res://Resources/Tools/Shovel.tres"),
			GD.Load<Tool>("res://Resources/Tools/Shears.tres")
		}
	);

	public override void _Ready()
	{
		_logger.Info("! Ready !");

		Game.Instance.Provide(this);

		_stateMachine = GetNode<PlayerStateMachine>("StateMachine");
		_stateMachine.Initialize(this);

		EventBus.Instance.OnItemPickUp += OnItemPickUp;

		// Initialize inventory
		var rand = new RandomStockGenerator();
		Inventory = new(20);
		Inventory.InventoryChanged += () => { EventBus.Instance.PlayerInventoryChanged(this, Inventory); };
		//Inventory.AddItem(rand.GetRandomItems(12).Where(it => it.Category == ItemCategory.Medicine).ToArray()); // For testing purposes
	}

	private void OnItemPickUp(IItemStack obj)
	{
		if (obj == null)
		{
			_logger.Error("Item is null, cannot pick up.");
			return;
		}

		var leftovers = Inventory.AddItem(obj);
		if (leftovers.Count > 0)
		{
			_logger.Warn("Inventory full, could not pick up all items. This is not handled");
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (GameStateMachine.Instance.CurrentState != GameState.FreeRoam)
		{
			_inputDirection = Vector2.Zero; // default value, movement is an exception
		}
	}

	public override void _Process(double delta)
	{
		UpdateFaceDirection();
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public void GetSetDirection()
	{
		_inputDirection = Input.GetVector(FreeRoam.Left, FreeRoam.Right, FreeRoam.Up, FreeRoam.Down).Normalized();
	}

	/// <summary>
	/// Update front direction (the direction the player is facing).
	/// The front direction equals the latest input direction. "Clamping" to NWSE (non-diagonal).
	/// The direction with the highest magnitude is the front direction.
	/// If there is no input direction, keep latest front direction value.
	/// </summary>
	private void UpdateFaceDirection()
	{
		if (Direction == Vector2.Zero)
		{
			return;
		}

		// Set the bigger one to 1/-1, the other to 0
		if (Math.Abs(Direction.X) > Math.Abs(Direction.Y))
		{
			_faceDirection = Vector2.Zero;
			_faceDirection.X = Direction.X;
		}
		else
		{
			_faceDirection = Vector2.Zero;
			_faceDirection.Y = Direction.Y;
		}

		_faceDirection = _faceDirection.Normalized();
	}

	public void UpdateAnimation(string animationName)
	{
		_playerAnimatedSprite?.UpdateAnimation(animationName);
	}
}
