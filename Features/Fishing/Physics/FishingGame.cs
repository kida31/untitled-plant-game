using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Fishing.Physics;

public partial class FishingGame : Node
{
	public event Action GameWon;

	[Export] private GameConfig _gameConfig;
	[Export] private Area2D _fishingPond;
	[Export] private ProgressBar _progressBar;
	[Export] private Bobber _fishingRod;
	[Export] private PackedScene _fishPrefab;
	[Export] private Marker2D _spawnPoint;
	[Export] private Node2D _gameWorld;

	private float _progress;
	private Fish _fish;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new(this);
		_fishingRod.Initialize(_gameConfig.HookWidth, _gameConfig.HookSpeed, _gameConfig.HookSpeedAgainstFish);
		// _fishingPond.AreaExited += OnFishingPondExited;
	}

	public override void _Process(double delta)
	{
		if (_progress >= 100)
		{
			// Victory
			_logger.Info("Victory!");
			_fish.QueueFree();
			_fish = null;
			_progress = 0;
			GameWon?.Invoke();
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		if (_fish != null && _fishingRod?.Fish != null && _fishingRod.ActiveDirection.X * _fish.ActiveDirection.X < 0)
		{
			_progress += (float)delta * _gameConfig.ProgressPullingPerSecond;
		}
		else
		{
			_progress -= (float)delta * _gameConfig.ProgressDecayPerSecond;
		}

		_progress = Math.Max(_progress, 0);

		// Update UI
		_progressBar.Value = _progress;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.F10)
		{
			_fish = SpawnFish();
		}
	}

	public Fish SpawnFish()
	{
		var direction = Vector2.Zero;
		direction.X = GD.Randf() - 0.5f;
		direction = direction.Normalized();

		_logger.Info($"Spawning a fish speed={_gameConfig.FishSpeed} pull={_gameConfig.PullSpeed} direction={direction}");

		var fish = _fishPrefab.Instantiate<Fish>();
		// fish.Initialize(_gameConfig.FishSpeed, _gameConfig.PullSpeed, direction);

		_gameWorld.AddChild(fish);
		fish.GlobalPosition = _spawnPoint.GlobalPosition;
		return fish;
	}

	private void OnFishingPondExited(Area2D area)
	{
		// if (area != _fish) return;

		_fish.QueueFree();
		_fish = null;
		_logger.Info("Fish escaped!");
	}
}
