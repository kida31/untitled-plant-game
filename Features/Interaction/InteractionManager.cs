using System.Collections.Generic;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.Inputs;
using untitledplantgame.Common.Inputs.GameActions;

/// <summary>
/// This Class manages the interaction with any interactable object in the game.
/// The Object registers itself to the InteractionManager once the player enters its area.
/// It unregisters itself once the player leaves it's area. The manager just sorts the list
/// of registered collisionshapes and chooses the closest one.
/// See AbstractNPC.cs
/// </summary>
public partial class InteractionManager : Node2D
{
	private string BaseText => $"[{InputRemapper.GetKey(FreeRoam.Interact).ToString()}] ";
	private const int BaseTextYTransform = 50;

	[Export]
	private Label label;
	public static InteractionManager Instance { get; private set; }
	public int AreaCount => activeAreas.Count;
	private Node2D player;
	private List<IInteractable> activeAreas = new();
	private bool canInteract = true;
	private readonly Logger _logger = new("InteractionManager");

	public override void _Ready()
	{
		player = (Node2D)GetTree().GetFirstNodeInGroup(GameGroup.Player);

		if (player == null)
		{
			_logger.Error("Player node not found.");
		}

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
		if (AreaCount > 0 && canInteract)
		{
			activeAreas.Sort(SortByDistanceToPlayer);
			label.Text = BaseText + activeAreas[0].ActionName;
			label.GlobalPosition = activeAreas[0].GetGlobalInteractablePosition();
			label.GlobalPosition -= new Vector2(label.Size.X / 2, BaseTextYTransform);
			label.Show();
		}
		else
		{
			label.Hide();
		}
	}

	public void RegisterArea(IInteractable area)
	{
		activeAreas.Add(area);
	}

	public void UnregisterArea(IInteractable area)
	{
		activeAreas.Remove(area);
	}

	public void PerformInteraction()
	{
		if (Input.IsActionJustPressed(FreeRoam.Interact) && canInteract)
		{
			if (AreaCount > 0)
			{
				canInteract = false;
				label.Hide();

				activeAreas[0].Interact();

				canInteract = true;
			}
		}
	}

	private int SortByDistanceToPlayer(IInteractable area1, IInteractable area2)
	{
		if (area1 == null || area2 == null)
		{
			_logger.Error("Area is null.");
			return int.MaxValue;
		}

		if (player == null)
		{
			_logger.Error("Player is null.");
			return int.MaxValue;
		}

		float distance1 = player.GlobalPosition.DistanceSquaredTo(area1.GetGlobalInteractablePosition());
		float distance2 = player.GlobalPosition.DistanceSquaredTo(area2.GetGlobalInteractablePosition());
		return distance1.CompareTo(distance2);
	}
}
