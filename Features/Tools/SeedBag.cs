using System;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Plants;
using untitledplantgame.Plants.Soil;

namespace untitledplantgame.Tools;

public class SeedBag : Tool
{
	public ItemStack CurrentSeedItem;
	
	private APlant _currentPlant;
	private Logger _logger;
	
	public SeedBag(float radius, float range, float channelingTime) : base(radius, range, channelingTime)
	{
		_logger = new Logger("SeedBag");
	}

	protected override void OnStart(Player.Player user)
	{
		var inventory = user.Inventory.GetInventory(ItemCategory.Seed);
		var items = inventory.GetItems();
		CurrentSeedItem = items.FirstOrDefault(it => it != null);
		
		if (CurrentSeedItem.Category != ItemCategory.Seed)
		{
			_logger.Error("There should only be plants in the seed bag");
		}
		
		//_currentPlant = PlantDatabase.Instance.GetResourceByName(CurrentSeedItem.Name).CreatePlant();
	}

	protected override bool OnInitialHit(Player.Player user, Node2D[] hits)
	{
		return hits.OfType<SoilTile>().Any();
	}

	protected override bool OnHit(Player.Player user, Node2D[] hits)
	{
		var closestTile = hits.OfType<SoilTile>().MinBy(p => p.GlobalPosition.DistanceSquaredTo(user.GlobalPosition));
		if (closestTile == null)
		{
			return false;
		}
		
		_currentPlant.PlantOnTile(closestTile);
		_logger.Debug($"Planted seed of {_currentPlant.PlantName} on tile");
		return true;
	}

	protected override void OnMiss(Player.Player user)
	{
		//play animation 
	}
}
