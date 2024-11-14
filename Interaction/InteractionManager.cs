using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;

public partial class InteractionManager : Node2D
{
	[Export]
	private Label label;
	public static InteractionManager Instance { get; private set; }
	public int AreaCount => activeAreas.Count;
	private Node2D player;

	private const string BaseText = "[E] to ";
	private List<IInteractable> activeAreas = new();
	private bool canInteract = true;
	private readonly Logger _logger = new("InteractionManager");

	public override void _Ready()
	{
		// player = (Node2D)GetTree().GetFirstNodeInGroup("Player"); //not working I think
		player = (Node2D)GetNode("/root/TestInventoryScene/Player"); //TODO: Make this work properly whithout hardcoding the path

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

	public void RegisterArea(IInteractable area)
	{
		activeAreas.Add(area);
	}

	public void UnregisterArea(IInteractable area)
	{
		int index = activeAreas.IndexOf(area);
		if (index != -1)
		{
			activeAreas.Remove(area);
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
			label.GlobalPosition -= new Vector2(label.Size.X / 2, 36);
			label.Show();
		}
		else
		{
			label.Hide();
		}
	}

	public void PerformInteraction()
	{
		if (Input.IsKeyPressed(Key.E) && canInteract)
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
		}

		if (player == null)
		{
			_logger.Error("Player is null.");
		}
		float distance1 = player.GlobalPosition.DistanceSquaredTo(area1.GetGlobalInteractablePosition());
		float distance2 = player.GlobalPosition.DistanceSquaredTo(area2.GetGlobalInteractablePosition());
		return distance1.CompareTo(distance2);
	}
}
