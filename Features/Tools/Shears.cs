using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants;

// NOTE: Make APlant implement IHarvestable instead of we have multiple harvestable objects
namespace untitledplantgame.Tools;

public class Shears : Tool
{
	private Logger _logger;

	public Shears(float radius, float range) : base(radius, range, 1.5f)
	{
		_logger = new("Shears");
	}

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

		closestPlant.Harvest();
		_logger.Debug("...pipiab");
		// TODO: move result to player inventory
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
