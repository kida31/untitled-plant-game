using System;
using System.Collections.Generic;

namespace untitledplantgame.Common;

/// <summary>
/// Global game object. Service locator like.
/// This singleton provides access to commonly used references.
/// E.g. Player object
/// The relevant objects usually register themselves
/// </summary>
public class Game
{
	// Singleton (keep this at the top)
	private static readonly Lazy<Game> LazySingleton = new(() => new Game());
	public static Game Instance => LazySingleton.Value;

	// Fields
	public static Player.Player Player => Instance.GetPlayer();

	private Player.Player _player;

	// FlexField for any other service providers. Just abused this if you want to try something quickly.
	private static Dictionary<Type, object> _services = new();

	private Game()
	{
	}

	public void Provide(Player.Player player)
	{
		_player = player;
	}

	public Player.Player GetPlayer()
	{
		return Instance._player;
	}

	public T? Get<T>() where T : class
	{
		return _services.GetValueOrDefault(typeof(T), null) as T;
	}

	public void Provide<T>(T service)
	{
		_services[typeof(T)] = service;
	}
}
