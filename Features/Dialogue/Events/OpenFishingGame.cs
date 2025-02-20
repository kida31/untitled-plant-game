using System;
using System.Linq;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Fishing;
using untitledplantgame.Fishing.Classic;
using untitledplantgame.Inventory;
using untitledplantgame.Item;
using untitledplantgame.Item.Components;

namespace untitledplantgame.Dialogue.Events;

/// <summary>
///		This DialogueEvent should be used for when the player tries to open the FishingGame via an interaction with the FishingSpot. 
/// </summary>
[GlobalClass]
public partial class OpenFishingGame : DialogueEvent
{
	[Export] private Array<GameConfig> _randomGameConfigsPool;
	[Export] private GameConfig _tutorialConfig; // default

	private FishingMiniGameSingleton _fishingGame;
	private Logger _logger;

	// bandaid solution
	private int _gameWonCount = 0;
	
	public override void Execute()
	{
		_logger = new Logger("OpenFishingGame");
		if (_fishingGame == null)
		{
			_fishingGame = FishingMiniGameSingleton.Instance;
			_fishingGame.GameWon += OnGameWon;
			_fishingGame.GameLost += OnGameLost;
		}

		_logger.Info("Starting fishing game");
		GameStateMachine.Instance.ChangeState(GameState.Fishing);

		GameConfig config;
		if (_gameWonCount < 2)
		{
			_logger.Info("Using tutorial config for first 2 games");
			config = _tutorialConfig;
		}
		else
		{
			config = _randomGameConfigsPool.PickRandom();
		}

		_logger.Info("Using config: " + config.GetPath());
		_fishingGame.Start(config);
		_fishingGame.Show();
	}

	private void OnGameWon()
	{
		_fishingGame!.Hide();
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
		_logger.Info("Fishing game won");

		// Give some random fish
		var items = ItemDatabase.Instance.GetAllItems();
		var fish = items.Where(i => i.GetComponent<TagsComponent>()?.Contains(TagsComponent.Tags.IsFish) ?? false).ToList();
		var random = new Random();
		var fishItem = fish.ElementAt(random.Next(fish.Count));
		EventBus.Instance.ItemPickedUp(fishItem);
		_gameWonCount++;
	}

	private void OnGameLost()
	{
		_fishingGame!.Hide();
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
	}
}
