using System.Collections.Generic;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.Inputs;
using untitledplantgame.Common.Inputs.GameActions;

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
	public static InteractionManager Instance { get; private set; }

	[Export] private Label _label;

	private int AreaCount => _activeAreas.Count;
	private string BaseText => $"[{InputRemapper.GetButton(FreeRoam.Interact).ToString()}] ";
	private bool _canInteract = true;
	private const int BaseTextYTransform = 50;
	private Node2D _player;
	private readonly List<IInteractable> _activeAreas = new();
	private readonly Logger _logger = new("InteractionManager");

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
	}

	/// <summary>
	/// Looks for the closest interactable area and displays that area's action name.
	/// </summary>
	/// <param name="delta"></param>
	public override void _Process(double delta)
	{
		_player ??= Game.Instance.GetPlayer();

		if (AreaCount > 0 && _canInteract & _player != null)
		{
			_activeAreas.Sort(SortByDistanceToPlayer);
			_label.Text = BaseText + _activeAreas[0].ActionName;
			_label.GlobalPosition = _activeAreas[0].GetGlobalInteractablePosition();
			_label.GlobalPosition -= new Vector2(_label.Size.X / 2, BaseTextYTransform);
			_label.Show();
		}
		else
		{
			_label.Hide();
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

		_canInteract = false;
		_label.Hide();
		_activeAreas[0].Interact();
		_canInteract = true;
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

		float distance1 = _player.GlobalPosition.DistanceSquaredTo(area1.GetGlobalInteractablePosition());
		float distance2 = _player.GlobalPosition.DistanceSquaredTo(area2.GetGlobalInteractablePosition());
		GD.PrintRich(distance2);
		return distance1.CompareTo(distance2);
	}
}
