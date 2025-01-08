using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Player;

public partial class SceneManager : Node
{
	public static SceneManager Instance { get; private set; }
	private readonly Logger _logger = new("SceneManager");
	private Node loadInto;
	private bool loading = false;


	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
			loadInto = GetTree().Root.GetNode("Main").GetNode("GameWorldContainer");
			//TODO: Load Map with bed in it
		}
		else
		{
			QueueFree();
		}
	}

	// currently uneccessary like most other methods here except TPP
	public void SwapScenes(string sceneToLoad, Door currentDoor)
	{
		var player = GetTree().GetNodesInGroup(GameGroup.Player)[0] as Player;
		if (loading)
		{
			_logger.Warn("SceneManager is already loading something");
			return;
		}

		loading = true;
		var newScene = LoadScene(sceneToLoad);
		_logger.Debug($"New scene: {newScene.Name}");

		if (newScene == null)
		{
			_logger.Warn("Failed to load scene");
			loading = false;
			return;
		}

		var connectingDoor = FindConnectingDoor(newScene, currentDoor.entryDoorName);
		if (connectingDoor == null)
		{
			_logger.Warn("Failed to find connecting door " + currentDoor.entryDoorName);
			loading = false;
			return;
		}

		var sceneToUnload = currentDoor.GetParent();
		EventBus.Instance.SceneChange(sceneToUnload, newScene);

		player.Hide();
		player.GlobalPosition = connectingDoor.GlobalPosition;
		player.Rotation = connectingDoor.Rotation;

		sceneToUnload.QueueFree();
		loadInto.AddChild(newScene);

		_logger.Debug("Scene changed");
		player.Show();
		loading = false;

 
		// _logger.Warn(GetTree(). GetNodesInGroup(GameGroup.Placeholder).ToString());


		// GetTree(). GetNodesInGroup("persistent").ForEach(node =>
		// {
		// 	if (node is IPersistent persistent)
		// 	{
		// 		Logic.checkPersist(persistent);
		// 	}
		// });
	}

	//Tele Port Player
	public void TPP(Door currentDoor)
	{
		var player = GetTree().GetNodesInGroup(GameGroup.Player)[0] as Player;
		var interactables = GetTree().GetNodesInGroup(GameGroup.Interactables);
		Vector2 targetPosition = Vector2.Zero;

		foreach(Node node in interactables)
		{
			if(node is Door door && door.entryDoorName == currentDoor.entryDoorName && door != currentDoor)
			{
				// _logger.Debug($"TPP to {door.GlobalPosition}");
				targetPosition = door.GlobalPosition;
				break;
			}
		}

        if (targetPosition != Vector2.Zero)
        {
            player.GlobalPosition = targetPosition;
            _logger.Debug($"Player moved to {targetPosition}");
        }
        else
        {
            _logger.Warn("No matching door found to move the player");
        }
	}

	private Node LoadScene(string scenePath)
	{
		if (ResourceLoader.Exists(scenePath))
		{
			var scene = GD.Load<PackedScene>(scenePath);
			if (scene != null)
			{
				return scene.Instantiate();
			}
		}
		_logger.Error($"Cannot load scene at path: {scenePath}");
		return null;
	}

	private Door FindConnectingDoor(Node newScene, string entryDoorName)
	{
		foreach (Node child in newScene.GetChildren())
		{
			if (child is Door door && door.entryDoorName == entryDoorName)
			{
				return door;
			}
		}
		return null;
	}
}
