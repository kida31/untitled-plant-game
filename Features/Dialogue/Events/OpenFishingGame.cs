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

[GlobalClass]
public partial class OpenFishingGame : DialogueEvent
{
	[Export] private Array<GameConfig> _randomGameConfigsPool;
	[Export] private GameConfig _config;

	private FishingMiniGameSingleton _fishingGame;
	private Logger _logger;

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
		_logger.Debug(_config.GetPath());
		_fishingGame.Start(_config);
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
	}

	private void OnGameLost()
	{
		_fishingGame!.Hide();
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
	}
}
