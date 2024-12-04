using System;
using Godot;

namespace untitledplantgame.Player;

/// <summary>
/// Global game object. Service locator like.
/// </summary>
public class Game
{
	// Singleton (keep this at the top)
	private static Lazy<Game> _lazyInstance => new Lazy<Game>(() => new Game());
	public static Game Instance => _lazyInstance.Value;
	
	// Fields
	public static Player Player => Instance.GetPlayer();
	
	private Player _player;

	private Game()
	{
		
	}

	public void Provide(Player player)
	{
		_player = player;
	}
	public Player GetPlayer()
	{
		return Instance._player;
	}
}
