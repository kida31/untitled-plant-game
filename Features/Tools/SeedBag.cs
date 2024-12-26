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
	
	private Logger _logger;
	
	public SeedBag(float radius, float range, float channelingTime) : base(radius, range, channelingTime)
	{
		_logger = new Logger("SeedBag");
	}

	protected override void OnStart(Player.Player user)
	{
		var inventory = user.Inventory.GetInventory(ItemCategory.Seed);
		CurrentSeedItem = inventory.FirstOrDefault();

		if(CurrentSeedItem == null)
		{
			_logger.Info("No seeds in the seed bag");
			return;
		}
		
		if (CurrentSeedItem.Category != ItemCategory.Seed)
		{
			_logger.Error("There should only be seeds in the seed bag");
		}
	}

	protected override bool OnInitialHit(Player.Player user, Node2D[] hits)
	{
		return hits.OfType<SoilTile>().Any();
	}

	protected override bool OnHit(Player.Player user, Node2D[] hits)
	{
		var closestTile = hits.OfType<SoilTile>().MinBy(p => p.GlobalPosition.DistanceSquaredTo(user.GlobalPosition));
		if (closestTile == null || closestTile.Plant != null)
		{
			return false;
		}

		if (CurrentSeedItem.GetComponent<SeedComponent>() == null)
		{
			_logger.Error("There's something wrong with this seed." + CurrentSeedItem);
		}
		
		var plantName = CurrentSeedItem.GetComponent<SeedComponent>().PlantName;
		var currentPlant = APlant.Create(plantName);
		
		closestTile.PlantPlant(currentPlant);
		closestTile.AddChild(currentPlant);
		var newSeedItem = (ItemStack) CurrentSeedItem.Clone();
		newSeedItem.Amount = 1;
		user.Inventory.RemoveItem(newSeedItem);
		
		
		_logger.Debug($"Planted seed of {currentPlant.PlantName} on tile");
		
		return true;
	}

	protected override void OnMiss(Player.Player user)
	{
		//play animation 
		//shake head
	}
}
