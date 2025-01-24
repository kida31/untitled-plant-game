using Godot;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common.GameStates;

public partial class DebugGameState : Label
{
	private Queue<GameState> _stateHistory = new();

	public override void _Ready()
	{
		GameStateMachine.Instance.StateChanged += OnStateChanged;
	}

	private void OnStateChanged(GameState _, GameState nextState)
	{
		// Log
		_stateHistory.Enqueue(nextState);

		// Update
		var recentStates = _stateHistory.Select((v, i) => $"{v.Name}({i})").TakeLast(5);
		Text = $"GameState: {string.Join(" -> ", recentStates)}";
	}
}
