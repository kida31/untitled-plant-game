using System;
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
		var config = _randomGameConfigsPool.PickRandom();
		_fishingGame.Start(config);
		_fishingGame.Show();
	}

	private void OnGameWon()
	{
		_fishingGame!.Hide();
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
		_logger.Info("Fishing game won");

		// Give some random fish
		var item = ItemDatabase.Instance.GetAllItems()[0].Clone();
		item.Name = "Some Fish" + new Random().Next();
		item.Category = ItemCategory.Material;
		item.AddComponent(new TagsComponent(TagsComponent.Tags.IsFish));
		EventBus.Instance.ItemPickedUp(item);
	}

	private void OnGameLost()
	{
		_fishingGame!.Hide();
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
	}
}
