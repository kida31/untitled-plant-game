using System.Collections.Generic;
using Godot;

namespace untitledplantgame.Player;

public partial class PlayerStateMachine : Node
{
	private List<State> _states;
	private State _prevState;
	private State _currState;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Disabled;
	}

	public override void _Process(double delta)
	{
		ChangeState(_currState.Process(delta));
	}

	public override void _PhysicsProcess(double delta)
	{
		ChangeState(_currState.Physics(delta));
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		ChangeState(_currState.HandleInput(@event));
	}

	public void Initialize(Player player)
	{
		_states = new List<State>();

		foreach (var child in GetChildren())
		{
			if (child.GetType().IsSubclassOf(typeof(State)))
			{
				var state = (State)child;
				state.Player = player;
				_states.Add((State)child);
			}
		}

		if (_states.Count > 0)
		{
			ChangeState(_states[0]);
			ProcessMode = ProcessModeEnum.Inherit;
		}
	}

	void ChangeState(State newState)
	{
		if (newState == null || newState == _currState)
			return;

		if (_currState != null)
			_currState.Exit();

		_prevState = _currState;
		_currState = newState;
		_currState.Enter();
	}
}
