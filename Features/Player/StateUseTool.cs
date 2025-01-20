using System;
using System.Collections.Generic;
using Godot;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.Tools;

namespace untitledplantgame.Player;

public partial class StateUseTool : State
{
	private State _idleState;
	private bool _queuingExit;
	private Dictionary<Type, string> _toolActions = new()
	{
		{ typeof(WateringCan), "water" },
		{ typeof(Shears), "shears" },
		{ typeof(SeedBag), "seedbag" },
		{ typeof(Shovel), "shovel" }
	};

	public override void _Ready()
	{
		_idleState = GetNode<State>("../Idle"); // string might be prone to error
	}

	public override void Enter()
	{
		_queuingExit = false;
		
		var tool = Player.Toolbelt.CurrentTool;
		var toolName = tool.GetType();

		UpdateToolAnimation(toolName);
		
		if (tool != null)
		{
			tool.StartChanneling(Player); // Should be a public method in player instead of property access
			tool.FinishedCasting += OnFinishedCasting;
		}
	}

	private void UpdateToolAnimation(Type toolType)
	{
		var toolAction = _toolActions[toolType];
		Player.UpdateAnimation(toolAction);
	}

	public override State Process(double delta)
	{
		Player.SetDirection();
		Player.Velocity = Vector2.Zero;

		if (!Input.IsActionPressed(FreeRoam.UseTool))
		{
			Player.Toolbelt.CurrentTool?.Cancel(Player);
			var tool = Player.Toolbelt.CurrentTool;
			UpdateToolAnimation(tool?.GetType());
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
		var tool = Player.Toolbelt.CurrentTool;
		if (tool != null)
		{
			tool.FinishedCasting -= OnFinishedCasting;
		}
	}
}
