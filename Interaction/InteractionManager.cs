using System.Collections.Generic;
using Godot;

public partial class InteractionManager : Node2D
{
	// [Export] private NodePath playerPath;
	// [Export] private NodePath labelPath;

	private Node2D player;
	private Label label;
	public static InteractionManager Instance { get; private set; }
	private const string BaseText = "[E] to ";
	private List<IInteractable> activeAreas = new();
	private bool canInteract = true;

	public override void _Ready()
	{
		player = (Node2D)GetTree().GetFirstNodeInGroup("Player");
		label = GetNode<Label>("Label");

		if (player == null)
		{
			GD.PrintErr("Player node not found.");
		}

		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			GD.PrintErr("Blub");
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
		if (activeAreas.Count > 0 && canInteract)
		{
			activeAreas.Sort(SortByDistanceToPlayer);
			label.Text = BaseText + activeAreas[0].GetActionName();
			label.GlobalPosition = activeAreas[0].GetGlobalInteractablePosition();
			label.GlobalPosition -= new Vector2(label.Size.X / 2, 36);
			label.Show();
		}
		else
		{
			label.Hide();
		}
	}

	private int SortByDistanceToPlayer(IInteractable area1, IInteractable area2)
	{
		float distance1 = player.GlobalPosition.DistanceTo(area1.GetGlobalInteractablePosition());
		float distance2 = player.GlobalPosition.DistanceTo(area2.GetGlobalInteractablePosition());
		return distance1.CompareTo(distance2);
	}

	public void PerformInteraction()
	{
		if (Input.IsKeyPressed(Key.E) && canInteract)
		{
			if (activeAreas.Count > 0)
			{
				canInteract = false;
				label.Hide();

				activeAreas[0].Interact();

				canInteract = true;
			}
		}
	}
}
