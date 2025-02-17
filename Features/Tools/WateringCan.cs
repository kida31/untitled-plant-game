using System;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants;

namespace untitledplantgame.Tools;

/// <summary>
///		A watering can. Waters plants.
/// </summary>
public partial class WateringCan : Tool
{
	[Export] private float _wateringAmount;
	[Export] private float _waterCapacity; // TODO: Implement
	[Export] private bool _isBottomless;

	private float _currentWaterLevel;
	private readonly Logger _logger = new("WateringCan");

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
