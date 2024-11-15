namespace untitledplantgame.Common;

/// <summary>
/// Possible states of the game.
/// <remarks>
/// This class is a pseudo enum with properties. (unused)
/// Class objects are nullable unlike enums. (alternatively create some "NONE" enum)
/// </remarks>
/// </summary>
public sealed class GameState
{
	public static readonly GameState Gameplay = new GameState();
	public static readonly GameState Book = new GameState();
	public static readonly GameState Config = new GameState();
	public static readonly GameState Dialogue = new GameState();

	private GameState() { }
}
