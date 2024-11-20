using System;
using System.Collections.Generic;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.ResourceData;

namespace untitledplantgame.Plants;

public enum GrowthStage
{
	Sprouting,
	Seedling,
	Vegetating,
	Budding,
	Flowering,
	Ripening,
	Dead,
}

public partial class APlant : Node2D
{
	[Export]
	public string PlantName { get; private set; }

	private float _absorptionRate = 100.0f;
	private float _consumptionRate = 30.0f;

	private AnimatedSprite2D _sprite2D;
	public SoilTile Tile { get; set; }

	[Export] public GrowthStage Stage { get; private set; } = GrowthStage.Sprouting;
	private bool _isHarvestable;

	private Dictionary<string, Requirement> _currentRequirements;

	private int _daysToGrow;
	private int _currentDay;

	private Logger _logger;

	public override void _Ready()
	{
		_sprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		SetRequirements(PlantName);
		_logger = new Logger(PlantName);
	}
	
	
	/// <summary>
	/// Executes the growth cycle of the plant by absorbing water, consuming water, and absorbing sun.
	/// Checks if the requirements are fulfilled to advance to the next stage.
	/// </summary>
	public void DoGrowthCycle()
	{
		if (Stage == GrowthStage.Dead)
			return;

		AbsorbWaterFromTile();
		ConsumeWater();
		AbsorbSun();

		if (!CheckRequirements())
			return;

		_currentDay++;
		AdvanceStage();
	}

	/// <summary>
	/// sets the plant on a tile
	/// </summary>
	/// <param name="soilTile"></param> the tile the plant is planted on
	public void PlantOnTile(SoilTile soilTile)
	{
		Tile = soilTile;
	}

	public void Harvest()
	{
		if (_isHarvestable)
		{
			_logger.Info($"Plant {PlantName} has been harvested.");
			Stage = Stage == GrowthStage.Ripening ? GrowthStage.Budding : --Stage;
			SetRequirements(PlantName);
			_logger.Info("plant has reached stage " + Stage);
		}
		else
		{
			_logger.Info($"Plant {PlantName} is not ready to be harvested.");
		}
	}
	/// <summary>
	/// Updates the requirements for the plant to grow for current stage.
	/// sets the days to grow and the current day count to 0.
	/// sets the plant name.
	/// sets the sprite to the current stage.
	/// </summary>
	private void SetRequirements(string plantName)
	{
		var plantData = PlantDatabase.Instance.GetResourceByName(plantName);
		var plantRequirements = new Dictionary<string, Requirement>();

		var plantDataRequirementsForStage = plantData.DataForGrowthStages[(int)Stage].GrowthRequirements;

		foreach (var data in plantDataRequirementsForStage)
		{
			plantRequirements[data.Name.ToString()] = new Requirement(data.MaxLevel, data.MinLevel);
		}

		_daysToGrow = plantData.DataForGrowthStages[(int)Stage].DaysToGrow;
		_isHarvestable = plantData.DataForGrowthStages[(int)Stage].IsHarvestable;
		_currentDay = 0;
		_currentRequirements = plantRequirements;
		PlantName = plantData._plantName;

		_sprite2D.Play(Stage.ToString());
	}

	/// <summary>
	/// returns if the current requirements for the plant are fulfilled
	/// </summary>
	private bool CheckRequirements()
	{
		var fulfilled = false;
		foreach (var requirement in _currentRequirements)
		{
			fulfilled = CheckRequirement(requirement.Key);
			if (!fulfilled)
				break;
		}

		_logger.Debug($"Requirement {fulfilled} for stage {Stage}, current day count at {_currentDay} of {_daysToGrow}.");

		return fulfilled && Stage != GrowthStage.Ripening && Stage != GrowthStage.Dead;
	}

	/// <summary>
	/// checks if the requirement is fulfilled
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	private bool CheckRequirement(string key)
	{
		var isFulfilled = _currentRequirements[key].IsFulfilled();
		_logger.Debug($"Checking requirement {key}. Requirement is {isFulfilled}.");
		return isFulfilled;
	}

	/// <summary>
	/// checks if the plant has grown enough to advance to the next stage
	/// </summary>
	private void AdvanceStage()
	{
		if (_currentDay < _daysToGrow)
			return;

		Stage++;
		_logger.Info($"Plant {PlantName} advanced to {Stage}.");
		SetRequirements(PlantName);
	}

	/// <summary>
	/// Absorbs water from the tile the plant is planted on.
	/// </summary>
	private void AbsorbWaterFromTile()
	{
		var waterReq = _currentRequirements.GetValueOrDefault(RequirementType.water.ToString());
		var waterAbsorbed = Tile.WithdrawHydration(_absorptionRate) + waterReq.CurrentLevel;

		waterReq.CurrentLevel = Math.Min(waterAbsorbed, waterReq.MaxLevel);

		_logger.Debug(RequirementType.water.ToString() + _currentRequirements.GetValueOrDefault(RequirementType.water.ToString()));
	}

	/// <summary>
	/// Consumes water from the current water level needed for the plant to grow and survive.
	/// </summary>
	private void ConsumeWater()
	{
		var waterReq = _currentRequirements.GetValueOrDefault(RequirementType.water.ToString());
		waterReq.CurrentLevel -= _consumptionRate;

		if (waterReq.CurrentLevel < 0)
		{
			SetUnalive();
		}
	}

	/// <summary>
	/// Absorbs sun from the environment and updates the current sun level.
	/// </summary>
	private void AbsorbSun()
	{
		var sunReq = _currentRequirements.GetValueOrDefault(RequirementType.sun.ToString());

		sunReq.CurrentLevel = Math.Min(sunReq.CurrentLevel + _absorptionRate, sunReq.MaxLevel);

		_logger.Info(RequirementType.sun.ToString() + _currentRequirements.GetValueOrDefault(RequirementType.sun.ToString()));
	}

	private void SetUnalive()
	{
		_sprite2D.Play("Dead");
		Stage = GrowthStage.Dead;
		_isHarvestable = false;
		_logger.Info($"Plant {PlantName} has died due to lack of water.");
	}
}
