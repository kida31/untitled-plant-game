using Godot;
using System;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Scenes;

public partial class BlurController : Node
{
	public static BlurController Instance { get; private set; }

	[Export] private float _blurStrength;
	[Export] private BlurEffect _blurEffect;

	private Logger _logger;

	public override void _Ready()
	{
		if (Instance != null)
		{
			QueueFree();
			return;
		}

		_logger = new Logger(this);
		Instance = this;
		
		GameStateMachine.Instance.StateChanged += OnGameStateChanged;
	}

	public override void _ExitTree()
	{
		GameStateMachine.Instance.StateChanged -= OnGameStateChanged;
		Instance = null;
	}

	private void OnGameStateChanged(GameState prev, GameState next)
	{
		if (next != GameState.FreeRoam)
		{
			_logger.Debug("Enable blur");
			EnableBlur();
		}
		else
		{
			_logger.Info("Disable blur");
			DisableBlur();
		}
	}

	// Currently no public use.
	private void EnableBlur()
	{
		_blurEffect.Strength = _blurStrength;
	}

	// Currently no public use.
	private void DisableBlur()
	{
		_blurEffect.Strength = 0;
	}
}
