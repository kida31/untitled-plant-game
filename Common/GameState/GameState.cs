namespace untitledplantgame.Common.GameState;

/// <summary>
/// Possible states of the game.
/// <remarks>
/// This class is a pseudo enum with properties. (unused)
/// Class objects are nullable unlike enums. (alternatively create some "NONE" enum)
/// </remarks>
/// </summary>
public sealed class GameState
{
	public static readonly GameState FreeRoam = new();
	public static readonly GameState Book = new();
	public static readonly GameState Config = new();
	public static readonly GameState Dialogue = new();

	private GameState() { }
}
