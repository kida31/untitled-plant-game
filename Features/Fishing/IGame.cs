using System;
using System.Runtime.CompilerServices;
using Godot;

namespace untitledplantgame.Fishing;

public interface IGame
{
	event Action GameWon;
	event Action GameLost;
	void Start(Resource config);
	void Stop();
}
