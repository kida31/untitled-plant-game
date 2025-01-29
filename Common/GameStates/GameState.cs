using System.Collections.Generic;

namespace untitledplantgame.Common.GameStates;

/// <summary>
/// Possible states of the game.
/// <remarks>
/// This class is a pseudo enum with properties. (unused)
/// Class objects are nullable unlike enums. (alternatively create some "NONE" enum)
/// </remarks>
/// </summary>
public sealed class GameState
{
	public static readonly GameState FreeRoam = new("freeroam");
	public static readonly GameState Book = new("book");
	public static readonly GameState Config = new("config");
	public static readonly GameState Dialogue = new("dialogue");
	public static readonly GameState Shop = new("shop");
	public static readonly GameState VendingMachine = new("VendingMachine");
	public static readonly GameState Crafting = new("crafting");
	public static readonly GameState Fishing = new("fishing");

	public readonly string Name;

	private GameState(string name)
	{
		Name = name;
	}

	public static IEnumerable<GameState> GetValues()
	{
		return new[] { FreeRoam, Book, Config, Dialogue, Shop, Crafting, VendingMachine };
	}

	public override string ToString()
	{
		return GetType().Name + "." + Name;
	}
}
