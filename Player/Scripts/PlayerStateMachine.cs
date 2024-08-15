using Godot;
using System;
using System.Linq;

public partial class PlayerStateMachine : Node
{
	private state[] states;
	private state prev_state;
	private state current_state;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Disabled;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void Initialize(Player player)
	{
		states = new state[] { };
		foreach (Node child in GetChildren())
		{
			if (child.GetType() == typeof(state))
				states.Append(child);
		}

		if (states.Length > 0)
		{
			states[0]._player = player;
			ChangeState(states[0]);
			ProcessMode = ProcessModeEnum.Inherit;
		}

}
	private void ChangeState(state new_state)
	{
		if (new_state == null || new_state == current_state) 
			return;
		if (current_state != null)
			current_state.Exit();

		prev_state = current_state;
		current_state = new_state;
		current_state.Enter();

	}
}
