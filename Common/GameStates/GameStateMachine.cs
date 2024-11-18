using System;
using Godot;

namespace untitledplantgame.Common.GameStates;

/// <summary>
/// Represents the current state of the game.
/// <remarks>
/// Usage
/// <code>
/// CurrentState //for current state
/// PreviousState //for previous state
/// ChangeState(newState) //to change state
/// RevertState() //to revert to previous state
/// >StateChanged += (previous, current) => { ... } //to subscribe to state changes
/// </code>
/// </remarks>
/// </summary>
public partial class GameStateMachine : Node
{
	/// <summary>
	/// Event that is triggered when the game state changes.
	/// The first argument is the previous state, the second argument is the new state.
	/// </summary>
	public event Action<GameStates.GameState, GameStates.GameState> StateChanged;

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

	public GameStates.GameState CurrentState => _currentState;
	public GameStates.GameState PreviousState => _previousState;

	private GameStates.GameState _currentState = GameStates.GameState.FreeRoam;
	private GameStates.GameState _previousState = null;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		if (_instance is null)
		{
			_instance = this;
		}
		else
		{
			_logger.Error("Instance already exists. Deleting this instance.");
			QueueFree();
		}
	}

	public void ChangeState(GameStates.GameState newState)
	{
		_logger.Info($"Change game state {_currentState} -> {newState}");
		_previousState = _currentState;
		_currentState = newState;
		StateChanged?.Invoke(_previousState, _currentState);
	}

	public void RevertState()
	{
		if (_previousState is null)
		{
			_logger.Error("No previous state to revert to. There might be a mistake with the control flow");
			return;
		}

		var temp = _currentState;
		_currentState = _previousState;
		_previousState = null;
		StateChanged?.Invoke(temp, _currentState);
	}
}
