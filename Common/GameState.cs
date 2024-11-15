namespace untitledplantgame.Common;

public class GameState
{
	public static readonly GameState Gameplay = new GameState();
	public static readonly GameState Book = new GameState();
	public static readonly GameState Config = new GameState();
	public static readonly GameState Dialogue = new GameState();

	private GameState()
	{
	}
}
