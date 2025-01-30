using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Fishing;

public partial class FishingMiniGameSingleton : Control, IGame
{
	public static FishingMiniGameSingleton Instance { get; private set; }

	public event Action GameWon;
	public event Action GameLost;

	[Export] private Node _fishingGameNode;

	private IGame _main;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new(this);

		if (Instance != null)
		{
			_logger.Error("Multiple instances of FishingMiniGame");
			QueueFree();
			return;
		}

		Instance = this;

		_main = _fishingGameNode as IGame;
		if (_main == null)
		{
			_logger.Error("Fishing game node does not implement expected interface");
			QueueFree();
			return;
		}

		_main.GameWon += () => GameWon?.Invoke();
		_main.GameLost += () => GameLost?.Invoke();
	}

	public void Start(Resource config) => _main.Start(config);
	public void Stop() => _main.Stop();
}
