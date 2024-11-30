using System;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants.Soil;

namespace untitledplantgame.Tools;

public class WateringCan : Tool
{
	private readonly float _wateringAmount;
	private readonly float _waterCapacity; // TODO: Implement
	private readonly bool _isBottomless;

	private float _currentWaterLevel;
	private readonly Logger _logger;

	public WateringCan(float wateringAmount, float waterCapacity, bool isBottomless, float radius, float range) : base(radius, range, 1.5f)
	{
		_wateringAmount = wateringAmount;
		_waterCapacity = waterCapacity;
		_isBottomless = isBottomless;
		_currentWaterLevel = 0;
		_logger = new Logger("WateringCan");
	}

	protected override bool OnInitialHit(Player.Player user, Node2D[] hits)
	{
		return hits.OfType<IWaterable>().Any();
	}

	protected override bool OnHit(Player.Player user, Node2D[] hits)
	{
		var closestSoil = hits
			.OfType<IWaterable>()
			.MinBy(o => (o as Node2D)!.GlobalPosition.DistanceSquaredTo(user.GlobalPosition));

		if (closestSoil == null)
		{
			return false;
		}

		_logger.Debug("Watering soil");

		if (_isBottomless)
		{
			closestSoil.AddWater(_wateringAmount);
		}
		else
		{
			var deltaWater = Math.Min(_wateringAmount, _currentWaterLevel);
			closestSoil.AddWater(deltaWater);
			_currentWaterLevel -= deltaWater; // Consider moving this to OnUse instead
		}

		return true;
	}

	protected override void OnMiss(Player.Player user)
	{
		_logger.Debug("Did not water anything. Waste of water");
	}

	protected override void OnStart(Player.Player user)
	{
		// Something. Example reduce water in tool
	}
}
