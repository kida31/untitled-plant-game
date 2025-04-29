using System;
using System.Linq;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Cycle.Weather;
using untitledplantgame.Inventory;
using untitledplantgame.Item;
using untitledplantgame.Item.Components;
using Array = System.Array;

namespace untitledplantgame.Plants;

public enum GrowthStage
{
	Seed,
	Sprouting,
	Seedling,
	Vegetating,
	Budding,
	Flowering,
	Ripening,
	Dead,
}

public partial class Plant : Area2D
{
	private const string PlantPath = "res://Features/Plants/PlantPrefab.tscn";
	private static readonly PackedScene PlantScene = GD.Load<PackedScene>(PlantPath);

	[Export(PropertyHint.Enum, "Chuberry,Licary,Drupoleaum")]
	public string PlantName { get; private set; }

	[Export] public GrowthStage Stage { get; private set; }
	[Export] public SoilTile Tile { get; set; }

	[Export] private RequirementDataForGrowthStage _currentRequirements;

	public event Action<Plant> BeforePlantRemoved;
	public event Action<Plant> PlantGrown;
	public event Action<Plant> PlantDied;

	private readonly Logger _logger;

	private bool _isHarvestable;
	private PlantData _plantData;
	private PlantDemand GetDemand(RequirementType requirementType) => _plantData.GetDemand(requirementType);
	private int _rootHealth;

	public Plant()
	{
		_logger = new Logger(this);
		_logger.Debug($"Plant {PlantName} has been planted.");
		AddToGroup(GameGroup.Plants);
		_logger = new Logger(this);
	}

	public static Plant Create(string plantName)
	{
		var plant = PlantScene.Instantiate<Plant>();
		plant.PlantName = plantName;
		return plant;
	}

	public override void _Ready()
	{
		_logger.Debug($"Plant {PlantName} is ready.");
		SetPlantData();
		if (_currentRequirements == null)
		{
			_logger.Debug($"Plant {PlantName} has no current requirements. Setting Requirements.");
			SetRequirements();
		}
		else
		{
			_isHarvestable = _currentRequirements.IsHarvestable;
		}
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
		AbsorbSun();
		CheckForRootRot();

		if (CheckRequirements())
		{
			AdvanceStage();
		}
	}

	/// <summary>
	/// Harvests the plant if it is harvestable.
	/// </summary>
	public IItemStack Harvest()
	{
		_logger.Debug($"Plant {PlantName} harvest attempt. Currently harvestable is {_isHarvestable}");
		if (!_isHarvestable) return null;

		_logger.Debug($"Plant {PlantName} has been harvested.");
		var harvestedItems = GetHarvestItem();

		Stage = Stage == GrowthStage.Ripening ? GrowthStage.Budding : --Stage;
		SetRequirements();
		_logger.Debug("plant has reached stage " + Stage);
		EventBus.Instance.OnPlantHarvested(this);

		return harvestedItems;
	}

	/// <summary>
	/// Removes the plant from the scene.
	/// </summary>
	public void RemovePlant()
	{
		BeforePlantRemoved?.Invoke(this);
		QueueFree();
	}

	/// <summary>
	/// Updates the requirements for the plant to grow for current stage.
	/// sets the days to grow and the current day count to 0.
	/// sets the plant name.
	/// </summary>
	private void SetRequirements()
	{
		_logger.Debug($"Setting requirements for plant {PlantName} with stage {Stage}.");
		if (_plantData.DataForGrowthStages.Length <= (int)Stage)
		{
			_logger.Error("Plant data does not contain data for the current stage.");
			return;
		}

		_isHarvestable = _plantData.DataForGrowthStages[(int)Stage].IsHarvestable;
		_currentRequirements = _plantData.DataForGrowthStages[(int)Stage];
		PlantName = _plantData.PlantName;
	}

	private void SetPlantData()
	{
		var plantData = PlantDatabase.Instance.GetResourceByName(PlantName);
		if (plantData == null)
		{
			_logger.Error($"Plant data for {PlantName} not found.");
			QueueFree();
			return;
		}

		_plantData = plantData;
		_rootHealth = _plantData.MaxRootHealth;
	}

	/// <summary>
	/// returns if the current requirements for the plant are fulfilled
	/// </summary>
	private bool CheckRequirements()
	{
		if (Stage is GrowthStage.Dead or GrowthStage.Ripening) return false;

		var fulfilled = false;
		foreach (var requirement in _currentRequirements.GrowthRequirements)
		{
			fulfilled = CheckRequirement(requirement.Type);
			if (!fulfilled)
				break;
		}

		_logger.Debug($"Requirement {fulfilled} for stage {Stage} on plant {PlantName}.");

		return fulfilled;
	}

	/// <summary>
	/// checks if the requirement is fulfilled
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	private bool CheckRequirement(RequirementType key)
	{
		var requirement = _currentRequirements.GrowthRequirements.FirstOrDefault(r => r.Type == key);
		if (requirement == null) return false;
		var isFulfilled = requirement.IsFulfilled();
		_logger.Debug($"Checking requirement {key}. Requirement is {isFulfilled}.");
		return isFulfilled;
	}

	/// <summary>
	/// checks if the plant has grown enough to advance to the next stage
	/// </summary>
	private void AdvanceStage()
	{
		Stage++;
		_logger.Info($"Plant {PlantName} advanced to {Stage}.");
		PlantGrown?.Invoke(this);
		SetRequirements();
	}

	/// <summary>
	/// Absorbs water from the tile the plant is planted on.
	/// </summary>
	private void AbsorbWaterFromTile()
	{
		var waterReq = _currentRequirements.GrowthRequirements.FirstOrDefault(r => r.Type == RequirementType.water);
		if (waterReq == null)
		{
			_logger.Error("Water requirement not found.");
			return;
		}

		var waterAbsorbed = Tile.WithdrawHydration(GetDemand(RequirementType.water).AbsorptionRate) + waterReq.CurrentLevel;

		waterReq.CurrentLevel = Math.Min(waterAbsorbed, waterReq.MaxLevel);
		ConsumeWater();

		_logger.Debug(
			$"The requirement for {nameof(RequirementType.water)} is currently at level {waterReq.CurrentLevel}");
	}

	/// <summary>
	/// Consumes water from the current water level needed for the plant to grow and survive.
	/// </summary>
	private void ConsumeWater()
	{
		var waterReq = _currentRequirements.GrowthRequirements.FirstOrDefault(r => r.Type == RequirementType.water);
		if (waterReq == null)
		{
			_logger.Error("Water requirement not found.");
			return;
		}

		waterReq.CurrentLevel -= GetDemand(RequirementType.water).ConsumptionRate;

		if (waterReq.CurrentLevel < 0)
		{
			SetUnalive();
			_logger.Debug($"Plant {PlantName} has died due to lack of water.");
		}
	}

	/// <summary>
	/// Absorbs sun from the environment and updates the current sun level.
	/// </summary>
	private void AbsorbSun()
	{
		var sunReq = _currentRequirements.GrowthRequirements.FirstOrDefault(r => r.Type == RequirementType.sun);
		if (sunReq == null)
		{
			_logger.Error("Sun requirement not found.");
			return;
		}

		sunReq.CurrentLevel = Math.Min(sunReq.CurrentLevel + GetSunAbsorptionRateBasedOnWeather(), sunReq.MaxLevel);
		sunReq.CurrentLevel -= GetDemand(RequirementType.sun).ConsumptionRate;

		if (sunReq.CurrentLevel < 0)
		{
			SetUnalive();
			_logger.Debug($"Plant {PlantName} has died due to lack of sun.");
		}
	}

	private void SetUnalive()
	{
		Stage = GrowthStage.Dead;
		_isHarvestable = false;
		PlantDied?.Invoke(this);
	}

	private IItemStack GetHarvestItem()
	{
		if (!_isHarvestable) return null;
		_logger.Debug($"Looking for harvested items for {PlantName} with stage {Stage}.");
		var comparable = new HarvestedComponent(PlantName, Stage);
		var itemStacks = ItemDatabase.Instance.GetAllItems()
			.Find(i =>
			{
				var component = i.GetComponent<HarvestedComponent>();
				return component != null && component.Equals(comparable);
			});

		_logger.Debug("Harvested items: " + itemStacks);

		return itemStacks;
	}

	private float GetSunAbsorptionRateBasedOnWeather()
	{
		var absorptionRate = GetDemand(RequirementType.sun).AbsorptionRate;
		return WeatherCycle.Instance.CurrentWeather switch
		{
			Weather.Sunny => absorptionRate * 1.5f,
			Weather.Cloudy => absorptionRate * 1.0f,
			Weather.Rainy or Weather.Snowy => absorptionRate * 0.5f,
			_ => absorptionRate
		};
	}

	private void CheckForRootRot()
	{
		if (!Tile.IsDrowning())
		{
			return;
		}

		_rootHealth--;
		if (_rootHealth <= 0)
		{
			SetUnalive();
		}
	}
}
