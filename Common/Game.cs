using System;
using Godot;

namespace untitledplantgame.Player;

/// <summary>
/// Global game object. Service locator like.
/// </summary>
public class Game
{
	// Singleton (keep this at the top)
	private static readonly Lazy<Game> lazy = new Lazy<Game>(() => new Game());
	public static Game Instance => lazy.Value;

	// Fields
	public static Player Player => Instance.GetPlayer();

	private Player _player;

	private Game()
	{
	}

	public void Provide(Player player)
	{
		GD.Print("Providing player to Game singleton");
		_player = player;
	}

	public Player GetPlayer()
	{
		return Instance._player;
	}
}
