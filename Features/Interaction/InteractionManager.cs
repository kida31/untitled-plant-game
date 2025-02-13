using System.Collections.Generic;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.ExtensionMethods;
using untitledplantgame.Common.GameStates;
using untitledplantgame.GUI.Interactions;

namespace untitledplantgame.Interaction;

/// <summary>
/// This Class manages the interaction with any interactable object in the game.
/// The Object registers itself to the InteractionManager once the player enters its area.
/// It unregisters itself once the player leaves it's area. The manager just sorts the list
/// of registered collisionshapes and chooses the closest one.
/// See AbstractNPC.cs
/// </summary>
public partial class InteractionManager : Node2D
{
	private const float InteractionCooldown = 1.0f;
	public static InteractionManager Instance { get; private set; }

	[Export] private InteractHint _label;
	[Export] private Vector2 _hintOffset = Vector2.Zero;

	private int AreaCount => _activeAreas.Count;
	private bool _canInteract = true;
	private Node2D _player;
	private readonly List<IInteractable> _activeAreas = new();
	private readonly Logger _logger = new("InteractionManager");

	private Timer _cooldownTimer;

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			_logger.Error("No Instance found");
			QueueFree();
		}

		// Add cooldown timer
		_cooldownTimer = new Timer()
		{
			OneShot = true,
			WaitTime = InteractionCooldown,
		};
		_cooldownTimer.Timeout += () =>
		{
			_logger.Debug("Enabling interaction.");
			_canInteract = GameStateMachine.Instance.CurrentState == GameState.FreeRoam;
		};
		AddChild(_cooldownTimer);
		
		GameStateMachine.Instance.StateChanged += OnStateChanged;
	}

	/// <summary>
	/// Looks for the closest interactable area and displays that area's action name.
	/// </summary>
	/// <param name="delta"></param>
	public override void _Process(double delta)
	{
		_player ??= Game.Instance.GetPlayer();

		if (AreaCount > 0 && _canInteract && _player != null && GameStateMachine.Instance.CurrentState == GameState.FreeRoam)
		{
			_activeAreas.Sort(SortByDistanceToPlayer);

			_label.Text = _activeAreas[0].ActionName;
			_label.GlobalPosition = _activeAreas[0].GetGlobalInteractablePosition() + _hintOffset;
			_label.FadeIn(0.1f);
		}
		else
		{
			_label.FadeOut(0.5f);
		}
	}

	private void OnStateChanged(GameState prev, GameState next)
	{
		if (next != GameState.FreeRoam)
		{
			_canInteract = false;
		}
		else
		{
			// If the player is in FreeRoam, they can interact again, but not instantly
			_cooldownTimer.Start();
		}
	}

	public void RegisterArea(IInteractable area)
	{
		_activeAreas.Add(area);
	}

	public void UnregisterArea(IInteractable area)
	{
		_activeAreas.Remove(area);
	}

	public void PerformInteraction()
	{
		if (!_canInteract || AreaCount <= 0)
		{
			return;
		}

		_logger.Debug("Disabling interaction.");
		_canInteract = false;
		_activeAreas[0].Interact();
		
		_cooldownTimer.Start();
	}

	private int SortByDistanceToPlayer(IInteractable area1, IInteractable area2)
	{
		if (area1 == null || area2 == null)
		{
			_logger.Error("Area is null.");
			return int.MaxValue;
		}

		if (_player == null)
		{
			_logger.Error("Player is null.");
			return int.MaxValue;
		}

		var distance1 = _player.GlobalPosition.DistanceSquaredTo(area1.GetGlobalInteractablePosition());
		var distance2 = _player.GlobalPosition.DistanceSquaredTo(area2.GetGlobalInteractablePosition());
		return distance1.CompareTo(distance2);
	}
}
