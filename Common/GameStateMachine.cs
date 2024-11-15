using System;
using Godot;

namespace untitledplantgame.Common;

public partial class GameStateMachine : Node
{
	public static GameStateMachine Instance
	{
		get
		{
			if (_instance is null)
			{
				throw new InvalidOperationException("GameStateMachine instance is not ready yet.");
			}

			return _instance;
		}
		private set => _instance = value;
	}

	private static GameStateMachine _instance;

	public GameState CurrentState => _currentState;
	public GameState PreviousState => _previousState;


	private GameState _currentState = GameState.Gameplay;
	private GameState _previousState = null;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		if (Instance is null)
		{
			Instance = this;
		}
		else
		{
			_logger.Error("Instance already exists. Deleting this instance.");
			QueueFree();
		}
	}

	public void ChangeState(GameState newState)
	{
		_logger.Info($"Change game state {_currentState} -> {newState}");
		_previousState = _currentState;
		_currentState = newState;
	}

	public void RevertState()
	{
		if (_previousState is null)
		{
			_logger.Error("No previous state to revert to. There might be a mistake with the control flow");
			return;
		}

		_currentState = _previousState;
		_previousState = null;
	}
}
