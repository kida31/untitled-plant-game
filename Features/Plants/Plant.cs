using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Database;
using untitledplantgame.Inventory;
using untitledplantgame.Item;
using untitledplantgame.Item.Components;

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
	public event Action<Plant> BeforePlantRemoved;
	public event Action<Plant> PlantGrown;

	private Dictionary<string, Requirement> _currentRequirements;
	private readonly Logger _logger;

	private bool _isHarvestable;
	private float _absorptionRate;
	private float _consumptionRate;

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
		SetRequirements();
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

		var plantData = PlantDatabase.Instance.GetResourceByName(PlantName);
		var plantRequirements = new Dictionary<string, Requirement>();
		_absorptionRate = plantData.AbsorptionRate;
		_consumptionRate = plantData.ConsumptionRate;

		if (plantData.DataForGrowthStages.Length <= (int)Stage)
		{
			_logger.Error("Plant data does not contain data for the current stage.");
			return;
		}

		var plantDataRequirementsForStage = plantData.DataForGrowthStages[(int)Stage].GrowthRequirements;

		foreach (var data in plantDataRequirementsForStage)
		{
			plantRequirements[data.Name.ToString()] = new Requirement(data.MaxLevel, data.MinLevel);
		}

		_isHarvestable = plantData.DataForGrowthStages[(int)Stage].IsHarvestable;

		_currentRequirements = plantRequirements;
		PlantName = plantData.PlantName;
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

		_logger.Debug($"Requirement {fulfilled} for stage {Stage}.");

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
		var waterReq = _currentRequirements.GetValueOrDefault(RequirementType.water.ToString());
		var waterAbsorbed = Tile.WithdrawHydration(_absorptionRate) + waterReq.CurrentLevel;

		waterReq.CurrentLevel = Math.Min(waterAbsorbed, waterReq.MaxLevel);
		ConsumeWater();

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

		_logger.Debug(RequirementType.sun.ToString() + _currentRequirements.GetValueOrDefault(RequirementType.sun.ToString()));
	}

	private void SetUnalive()
	{
		Stage = GrowthStage.Dead;
		_isHarvestable = false;
		_logger.Debug($"Plant {PlantName} has died due to lack of water.");
	}

	private IItemStack GetHarvestItem()
	{
		if (!_isHarvestable) return null;
		_logger.Debug($"Looking for harvested items for {PlantName} with stage {Stage}.");
		var itemStacks = ItemDatabase.Instance.ItemStacks.FirstOrDefault(i =>
			i.GetComponent<HarvestedComponent>().Equals(new HarvestedComponent(PlantName, Stage)));

		_logger.Debug("Harvested items: " + itemStacks);

		return itemStacks;
	}
}
