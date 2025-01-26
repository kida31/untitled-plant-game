using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Item.Components;
using untitledplantgame.Plants;

namespace untitledplantgame.Tools;

[GlobalClass]
public partial class SeedBag : Tool
{
	private IItemStack _currentSeedItem;

	private readonly Logger _logger = new("SeedBag");

	protected override void OnStart(Player.Player user)
	{
		_currentSeedItem = null;

		var inventory = user.Inventory.GetInventory(ItemCategory.Seed);
		foreach (var item in inventory)
		{
			if (item == null)
			{
				continue;
			}

			_currentSeedItem = item;
			_logger.Debug($"Current seed in seed bag: {_currentSeedItem}");
			return;
		}

		if (_currentSeedItem?.Category != ItemCategory.Seed)
		{
			_logger.Error("There should only be seeds in the seed bag");
		}

		_logger.Debug("No seed in the seed bag");
	}

	protected override bool OnInitialHit(Player.Player user, Node2D[] hits)
	{
		var closestTile = hits.OfType<SoilTile>().MinBy(p => p.GlobalPosition.DistanceSquaredTo(user.GlobalPosition));
		if (closestTile == null)
		{
			_logger.Warn("There is no soil tile to plant on");
			return false;
		}

		if (IsSoilOccupied(closestTile))
		{
			return false;
		}

		return hits.OfType<SoilTile>().Any();
	}

	protected override bool OnHit(Player.Player user, Node2D[] hits)
	{
		var closestTile = hits.OfType<SoilTile>().MinBy(p => p.GlobalPosition.DistanceSquaredTo(user.GlobalPosition));
		if (closestTile == null)
		{
			_logger.Warn("There is no soil tile to plant on");
			return false;
		}

		if (IsSoilOccupied(closestTile))
		{
			return false;
		}

		if (_currentSeedItem == null)
		{
			_logger.Info("No seed in the seed bag");
			return false;
		}

		if (_currentSeedItem.GetComponent<SeedComponent>() == null)
		{
			_logger.Error("There's something wrong with this seed." + _currentSeedItem);
		}

		var plantName = _currentSeedItem.GetComponent<SeedComponent>().PlantName;
		var currentPlant = Plant.Create(plantName);
		closestTile.PlantSeed(currentPlant);

		var newSeedItem = _currentSeedItem.Clone();
		newSeedItem.Amount = 1;
		user.Inventory.RemoveItem(newSeedItem);

		_logger.Debug($"Planted seed of {currentPlant.PlantName} on tile");

		return true;
	}

	private bool IsSoilOccupied(SoilTile closestTile)
	{
		var plants = closestTile.GetTree().GetNodesInGroup(GameGroup.Plants).OfType<Plant>().ToArray();
		foreach (var plant in plants)
		{
			if (plant.Tile != closestTile)
			{
				continue;
			}

			_logger.Warn($"Soil tile is already occupied by {plant.PlantName}");
			return true;
		}

		return false;
	}

	protected override void OnMiss(Player.Player user)
	{
		//play animation 
		//shake head
	}
}
