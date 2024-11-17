using System.Collections.Generic;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameState;
using untitledplantgame.Player;

public partial class InteractablesManager : Node
{
	[Export]
	private Player _player;

	private List<IInteractable> _interactablesInReach = new();
	private IInteractable _nearestInteractable;

	// In Player
	public void ScanForInteractables()
	{
		if (GameStateMachine.Instance.CurrentState != GameState.FreeRoam)
		{
			return;
		}

		CheckForInteractables();
		if (Input.IsKeyPressed(Key.E)) // Bind this to your interact key
		{
			PerformInteraction();
		}
	}

	private void CheckForInteractables()
	{
		// Clear previous interactables
		_interactablesInReach.Clear();

		// Get all Area2D nodes that are in range (this could be optimized)
		var area2DList = GetTree().GetNodesInGroup("Interactables");

		foreach (var area in area2DList)
		{
			if (area is IInteractable interactable)
			{
				if (IsOverlapping(interactable as Area2D))
				{
					_interactablesInReach.Add(interactable);
				}
			}
		}

		// Find the nearest interactable
		_nearestInteractable = FindNearestInteractable();
	}

	private bool IsOverlapping(Area2D area)
	{
		return area.GetOverlappingBodies().Contains(_player);
	}

	private IInteractable FindNearestInteractable()
	{
		IInteractable closest = null;
		float closestDistance = float.MaxValue;

		foreach (var interactable in _interactablesInReach)
		{
			float distance = (interactable.GetGlobalInteractablePosition() - _player.GlobalPosition).Length();
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closest = interactable;
			}
		}

		return closest;
	}

	private void PerformInteraction()
	{
		_nearestInteractable?.Interact();
	}
}
