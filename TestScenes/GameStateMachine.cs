using Godot;

namespace untitledplantgame.TestScenes;

public partial class GameStateMachine: Node
{
	public static GameStateMachine Instance { get; private set; }
	public GameState CurrentState { get; set; } = GameState.GAMEPLAY;
	
	public override void _Ready()
	{
		if (Instance is null)
		{
			Instance = this;
		}
		else
		{
			QueueFree();
		}
	}
}
