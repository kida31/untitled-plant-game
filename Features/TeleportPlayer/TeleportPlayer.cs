using Godot;
using untitledplantgame.Common;
using untitledplantgame.Player;

public partial class TeleportPlayer : Node
{
	public static TeleportPlayer Instance { get; private set; }
	private readonly Logger _logger = new("TeleportPlayer");

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			QueueFree();
		}
	}


	async public void TPP(Door currentDoor)
	{
		var player = GetTree().GetNodesInGroup(GameGroup.Player)[0] as Player;

		var interactables = GetTree().GetNodesInGroup(GameGroup.Interactables);
		Vector2 targetPosition = Vector2.Zero;
		var trans = GetTree().Root.GetNode("Main").GetNode("GUIContainer").GetNode<SceneTransition>("SceneTransition");

		foreach (Node node in interactables)
		{
			if (node is Door door && door.entryDoorName == currentDoor.entryDoorName && door != currentDoor)
			{
				targetPosition = door.GlobalPosition;
				break;
			}
		}

		if (targetPosition != Vector2.Zero)
		{
			await trans.FadeIn();
			player!.GlobalPosition = targetPosition;
			await trans.FadeOut();

			_logger.Debug($"Player moved to {targetPosition}");
		}
		else
		{
			_logger.Warn("No matching door found to move the player");
		}
	}
}
