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
	public event Action<GameState, GameState> StateChanged;

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

	private GameState _currentState = GameState.FreeRoam;
	private GameState _previousState;
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

	/// <summary>
	/// Changes the game state.
	/// </summary>
	/// <param name="newState"></param>
	public void SetState(GameState newState)
	{
		_logger.Info($"Change game state {_currentState} -> {newState}");
		_previousState = _currentState;
		_currentState = newState;
		StateChanged?.Invoke(_previousState, _currentState);
	}

	/// <summary>
	/// Changes the game state.
	/// </summary>
	/// <param name="newState"></param>
	public void ChangeState(GameState newState) => SetState(newState);

	[Unstable("Seems to cause errors sometimes for unknown reason.")]
	private void RevertState()
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
