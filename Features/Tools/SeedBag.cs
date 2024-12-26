using System.Diagnostics;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Database;
using untitledplantgame.Inventory;
using untitledplantgame.Plants;
using untitledplantgame.Plants.Soil;
using SeedComponent = untitledplantgame.Item.Components.SeedComponent;

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
		CurrentSeedItem = inventory.FirstOrDefault();

		Debug.Assert(CurrentSeedItem != null, nameof(CurrentSeedItem) + " != null");
		
		if (CurrentSeedItem.Category != ItemCategory.Seed)
		{
			_logger.Error("There should only be seeds in the seed bag");
		}

		var plantName = CurrentSeedItem.Components.OfType<SeedComponent>().First().PlantName;
		_currentPlant = APlant.Create(plantName);
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
		closestTile.AddChild(_currentPlant);
		var newSeedItem = (ItemStack) CurrentSeedItem.Clone();
		newSeedItem.Amount = 1;
		user.Inventory.RemoveItem(newSeedItem);
		
		
		_logger.Debug($"Planted seed of {_currentPlant.PlantName} on tile");
		
		return true;
	}

	protected override void OnMiss(Player.Player user)
	{
		//play animation 
	}
}
