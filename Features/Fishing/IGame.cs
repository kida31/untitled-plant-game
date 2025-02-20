using System;
using System.Runtime.CompilerServices;
using Godot;

namespace untitledplantgame.Fishing;

/// <summary>
///		A mini game interface. A game can be started and stopped.
///		The game has events that are emitted when the game is won or lost.
/// </summary>
public interface IGame
{
	event Action GameWon;
	event Action GameLost;
	void Start(Resource config);
	void Stop();
}
