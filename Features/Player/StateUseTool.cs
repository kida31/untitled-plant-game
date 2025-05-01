using System;
using System.Collections.Generic;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.Tools;

namespace untitledplantgame.Player;

public partial class StateUseTool : State
{
	private State _idleState;
	private bool _queuingExit;
	private Logger _logger;

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
		_logger = new(this);
	}

	public override void Enter()
	{
		_queuingExit = false;

		var tool = Player.Toolbelt.CurrentTool;
		
		if (tool == null)
		{
			_logger.Error("Tool is null. Should not be in this state");
			return;
		}

		var toolName = tool.GetType();

		UpdateToolAnimation(toolName);
		tool.StartChanneling(Player); // Should be a public method in player instead of property access
		tool.FinishedCasting += OnFinishedCasting;
		tool.HitRegistered += OnToolHit;
	}

	private void UpdateToolAnimation(Type toolType)
	{
		var toolAction = _toolActions[toolType];
		Player.UpdateAnimation(toolAction);
	}

	public override State Process(double delta)
	{
		Player.GetSetDirection();
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
		Player.GetSetDirection();
		return null;
	}

	public override void Exit()
	{
		var tool = Player.Toolbelt.CurrentTool;
		if (tool != null)
		{
			tool.FinishedCasting -= OnFinishedCasting;
			tool.HitRegistered -= OnToolHit;
		}
	}

	private void OnFinishedCasting()
	{
		_queuingExit = true;
	}

	private void OnToolHit()
	{
		var tool = Player?.Toolbelt?.CurrentTool;
		var tType = tool.GetType();

		AudioStreamWav sfx;
		if (tType == typeof(WateringCan))
		{
			sfx = GD.Load<AudioStreamWav>("res://Assets/SFX/Tools/WaterPlants.wav");
		}
		else if (tType == typeof(Shears))
		{
			sfx = GD.Load<AudioStreamWav>("res://Assets/SFX/Tools/CuttingPlants.wav");
		}
		else if (tType == typeof(SeedBag))
		{
			sfx = GD.Load<AudioStreamWav>("res://Assets/SFX/Tools/SowSeeds.wav");
		}
		else if (tType == typeof(Shovel))
		{
			sfx = GD.Load<AudioStreamWav>("res://Assets/SFX/Tools/DiggingSounds.wav");
		} else 
		{
			_logger.Error("Failed to load Tool SFX");
			return;
		}


		Player.PlaySfx(sfx);
	}
}
