using Godot;
using System;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Plants;
using untitledplantgame.Player;
using untitledplantgame.Tools;

public class Shovel : Tool
{
	private Logger _logger;
	
	public Shovel(float radius, float range, float channelingTime) : base(radius, range, channelingTime)
	{
		_logger = new Logger("Shovel");
	}

	protected override void OnStart(Player user)
	{
		_logger.Debug("Digging...");
	}

	protected override bool OnInitialHit(Player user, Node2D[] hits)
	{
		return hits.OfType<Plant>().Any();
	}

	protected override bool OnHit(Player user, Node2D[] hits)
	{
		var closestPlant = hits.OfType<Plant>().MinBy(p => p.GlobalPosition.DistanceSquaredTo(user.GlobalPosition));
		if (closestPlant == null)
		{
			return false;
		}

		closestPlant.RemovePlant();
		return true;
	}

	protected override void OnMiss(Player user)
	{
		_logger.Debug("There is no Plant to remove.");
	}
}
