using System;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants.Soil;

namespace untitledplantgame.Tools;

public class WateringCan : Tool
{
	// Placeholders
	private const int PlaceholderWateringAmount = 20;
	private const float WateringRadius = 16;

	private const float WateringRange = 16; // TODO: Fix; Direction of player is only active while moving instead of most recent facing
	// EndPlaceholders

	private readonly float _wateringAmount;
	private readonly float _waterCapacity;
	private readonly bool _isBottomless;

	private float _currentWaterLevel;
	private readonly Logger _logger;

	public WateringCan() : this(WateringRadius, WateringRange)
	{
		_wateringAmount = PlaceholderWateringAmount;
	}

	public WateringCan(float radius, float range) : base(radius, range)
	{
		_logger = new Logger("WateringCan");
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
		var deltaWater = Math.Min(_wateringAmount, _currentWaterLevel);
		closestSoil.AddWater(deltaWater);
		
		// Consider moving this to OnUse instead
		if (!_isBottomless)
		{
			_currentWaterLevel -= deltaWater;
		}

		return true;
	}

	protected override void OnMiss(Player.Player user)
	{
		_logger.Debug("Did not water anything. Waste of water");
	}

	protected override void OnUse(Player.Player user)
	{
		// Something. Example reduce water in tool
	}
}
