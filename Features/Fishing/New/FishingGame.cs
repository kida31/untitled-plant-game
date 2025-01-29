using System;
using Godot;

namespace untitledplantgame.Fishing.New;

public partial class FishingGame : Node
{
	[Export] private GameConfig _gameConfig;
	[Export] private FishingPond _fishingPond;
	[Export] private ProgressBar _progressBar;
	[Export] private FishingRod _fishingRod;

	private float _progress;
	private Fish _fish;

	public override void _Ready()
	{
		_fishingRod.Initialize(_gameConfig.HookWidth, _gameConfig.HookSpeed, _gameConfig.HookSpeedAgainstFish);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_fishingRod?.Fish != null && _fishingRod.ActiveDirection.X * _fish.ActiveDirection.X < 0)
		{
			_progress += (float)delta * _gameConfig.ProgressPullingPerSecond;
		}
		else
		{
			_progress -= (float)delta * _gameConfig.ProgressDecayPerSecond;
		}
	
		_progress = Math.Clamp(_progress, 0, 100);


		// Update UI
		_progressBar.Value = _progress;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.F10)
		{
			var direction = Vector2.Zero;
			direction.X = GD.Randi() - 0.5f;
			_fish = _fishingPond.SpawnFish(_gameConfig.FishSpeed, _gameConfig.PullSpeed, direction);
		}
	}
}
