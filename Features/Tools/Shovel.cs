using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants;

namespace untitledplantgame.Tools;

public partial class Shovel : Tool
{
	private readonly Logger _logger = new("Shovel");

	protected override void OnStart(Player.Player user)
	{
		_logger.Debug("Digging...");
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

		closestPlant.RemovePlant();
		return true;
	}

	protected override void OnMiss(Player.Player user)
	{
		_logger.Debug("There is no Plant to remove.");
	}
}
