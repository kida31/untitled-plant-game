using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants;
using untitledplantgame.Player;
using untitledplantgame.Tools;

// NOTE: Make APlant implement IHarvestable instead of we have multiple harvestable objects
public class Shears : Tool
{
	private Logger _logger;

	public Shears(float radius, float range) : base(radius, range, 1.5f)
	{
		_logger = new("Shears");
	}

	protected override bool OnInitialHit(Player user, Node2D[] hits)
	{
		return hits.OfType<APlant>().Any();
	}

	protected override bool OnHit(Player user, Node2D[] hits)
	{
		var closestPlant = hits.OfType<APlant>().MinBy(p => p.GlobalPosition.DistanceSquaredTo(user.GlobalPosition));
		if (closestPlant == null)
		{
			return false;
		}

		closestPlant.Harvest();
		_logger.Debug("...pipiab");
		// TODO: move result to player inventory
		return true;
	}

	protected override void OnMiss(Player user)
	{
		// pass
	}

	protected override void OnStart(Player user)
	{
		_logger.Debug("Schnippschnapp...");
	}
}
