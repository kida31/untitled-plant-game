using System;
using Godot;
using untitledplantgame.Common.Inputs.GameActions;

namespace untitledplantgame.Player;

public partial class StateUseTool : State
{
	private State _idleState;
	private bool _queuingExit;

	public override void _Ready()
	{
		_idleState = GetNode<State>("../Idle"); // string might be prone to error
	}

	public override void Enter()
	{
		_queuingExit = false;
		Player.UpdateAnimation("idle");

		var tool = Player.Toolbelt.CurrentTool;
		if (tool != null)
		{
			tool.StartChanneling(Player); // Should be a public method in player instead of property access
			tool.FinishedCasting += OnFinishedCasting;
		}
	}

	public override State Process(double delta)
	{
		Player.SetDirection();
		Player.Velocity = Vector2.Zero;
		Player.UpdateAnimation("idle");

		if (!Input.IsActionPressed(FreeRoam.UseTool))
		{
			Player.Toolbelt.CurrentTool?.Cancel(Player);
			return _idleState;
		}

		return _queuingExit ? _idleState : null;
	}

	public override State HandleInput(InputEvent inputEvent)
	{
		Player.GetSetInputDirection();
		return null;
	}

	private void OnFinishedCasting()
	{
		_queuingExit = true;
	}

	public override void Exit()
	{
		GD.Print("Bye");
		var tool = Player.Toolbelt.CurrentTool;
		if (tool != null)
		{
			tool.FinishedCasting -= OnFinishedCasting;
		}
	}
}
