using System.Linq;
using Godot;
using Godot.NativeInterop;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Plants;
using SeedComponent = untitledplantgame.Item.Components.SeedComponent;

namespace untitledplantgame.Tools;

public class SeedBag : Tool
{
	public IItemStack CurrentSeedItem;
	
	private Logger _logger;
	
	public SeedBag(float radius, float range, float channelingTime) : base(radius, range, channelingTime)
	{
		_logger = new Logger("SeedBag");
	}

	protected override void OnStart(Player.Player user)
	{
		var inventory = user.Inventory.GetInventory(ItemCategory.Seed);
		CurrentSeedItem = inventory.FirstOrDefault() as ItemStack; // This might break if ItemStack is not properly updated (independent of IItemstack)

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
		
		if(CurrentSeedItem == null)
		{
			_logger.Info("No seed in the seed bag");
			return false;
		}

		if (CurrentSeedItem.GetComponent<SeedComponent>() == null)
		{
			_logger.Error("There's something wrong with this seed." + CurrentSeedItem);
		}
		
		var plantName = CurrentSeedItem.GetComponent<SeedComponent>().PlantName;
		var currentPlant = Plant.Create(plantName);
		closestTile.PlantSeed(currentPlant);
		
		var newSeedItem = (ItemStack) CurrentSeedItem.Clone();
		newSeedItem.Amount = 1;
		user.Inventory.RemoveItem(newSeedItem);
		
		_logger.Debug($"Planted seed of {currentPlant.PlantName} on tile");
		
		return true;
	}

	private bool IsSoilOccupied(SoilTile closestTile)
	{
		var plants = closestTile.GetTree().GetNodesInGroup(GameGroup.Plants).OfType<Plant>();
		var enumerable = plants as Plant[] ?? plants.ToArray();
		var b = enumerable.Any(plant => plant.Tile == closestTile);
		if (b)
		{
			_logger.Warn($"Soil tile is already occupied by {enumerable.First().PlantName}");
		}
		return b;
	}

	protected override void OnMiss(Player.Player user)
	{
		//play animation 
		//shake head
	}
}
