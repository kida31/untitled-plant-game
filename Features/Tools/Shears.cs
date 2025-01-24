using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants;

// NOTE: Make APlant implement IHarvestable instead of we have multiple harvestable objects
namespace untitledplantgame.Tools;

public partial class Shears : Tool
{
	private readonly Logger _logger = new("Shears");

	protected override bool OnInitialHit(Player.Player user, Node2D[] hits)
	{
		return hits.OfType<Plant>().Any();
	}

	protected override bool OnHit(Player.Player user, Node2D[] hits)
	{
		var closestPlant = hits.OfType<Plant>().MinBy(p => p.GlobalPosition.DistanceSquaredTo(user.GlobalPosition));
		if (closestPlant == null)
		{
			return false;
		}

		var harvestedItems = closestPlant.Harvest();
		if(harvestedItems == null)
		{
			_logger.Debug("No items were harvested from the plant");
			return false;
		}
		
		user.Inventory.AddItem(harvestedItems.ToArray());
		return true;
	}

	protected override void OnMiss(Player.Player user)
	{
		// pass
	}


	protected override void OnStart(Player.Player user)
	{
		_logger.Debug("Schnippschnapp...");
	}
}
