using System;
using Godot;
using System.Collections.Generic;
using untitledplantgame.Common;
using untitledplantgame.MagicBoxForData;

namespace untitledplantgame.Plants;

public enum GrowthStage
{
    Seedling,
    Vegetating,
    Budding,
    Flowering,
    Ripening,
    Dead,
}

public partial class APlant : Node2D
{
    public string PlantName { get; private set; }
    [Export] private int _plantId;

    private float _absorptionRate = 100.0f;
    private float _consumptionRate = 30.0f;

    private AnimatedSprite2D _sprite2D;
    public SoilTile Tile { get; set; }

    [Export] public GrowthStage Stage { get; private set; } = GrowthStage.Seedling;

    private Dictionary<string, Requirement> _currentRequirements;

    private int _daysToGrow;
    private int _currentDay;

    private Logger _logger;

    public override void _Ready()
    {
        _sprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _plantId = 0;
        UpdateRequirements();
        _logger = new Logger(PlantName);
    }

    private void UpdateRequirements()
    {
        var plantData = ResourceManager.Instance.GetPlantData(_plantId);
        var plantRequirements = new Dictionary<string, Requirement>();

        var plantDataRequirementsForStage = plantData.DataForGrowthStages[(int)Stage].GrowthRequirements;

        foreach (var data in plantDataRequirementsForStage)
        {
            plantRequirements[data.Name.ToString()] = new Requirement(data.MaxLevel, data.MinLevel);
        }

        _daysToGrow = plantData.DataForGrowthStages[(int)Stage].DaysToGrow;
        _currentRequirements = plantRequirements;
        PlantName = plantData._plantName;

        _sprite2D.Play(Stage.ToString());
    }

    private void CheckRequirements()
    {
        var fulfilled = false;
        foreach (var requirement in _currentRequirements)
        {
            fulfilled = CheckRequirement(requirement.Key);
            if (!fulfilled) break;
        }
        
        _logger.Debug(
            $"Requirement {fulfilled} for stage {Stage}, current day count at {_currentDay} of {_daysToGrow}.");

        if (!fulfilled || Stage == GrowthStage.Ripening || Stage == GrowthStage.Dead) return;

        _currentDay++;
        AdvanceStage();
    }

    private bool CheckRequirement(string key)
    {
        var isFulfilled = _currentRequirements[key].IsFulfilled();
        _logger.Debug($"Checking requirement {key}. Requirement is {isFulfilled}.");
        return isFulfilled;
    }

    private void AdvanceStage()
    {
        if (_currentDay < _daysToGrow) return;

        Stage++;
        _logger.Info($"Plant {PlantName} advanced to {Stage}.");
        UpdateRequirements();
    }

    public void PlantOnTile(SoilTile soilTile)
    {
        Tile = soilTile;
    }

    private void AbsorbWaterFromTile()
    {
        var waterReq = _currentRequirements.GetValueOrDefault(RequirementType.water.ToString());
        var waterAbsorbed = Tile.WithdrawHydration(_absorptionRate) + waterReq.CurrentLevel;

        waterReq.CurrentLevel = Math.Min(waterAbsorbed, waterReq.MaxLevel);

        _logger.Debug(RequirementType.water.ToString() +
                      _currentRequirements.GetValueOrDefault(RequirementType.water.ToString()));
    }

    private void ConsumeWater()
    {
        var waterReq = _currentRequirements.GetValueOrDefault(RequirementType.water.ToString());
        waterReq.CurrentLevel -= _consumptionRate;

        if (waterReq.CurrentLevel < 0)
        {
            DryUp();
        }
    }

    private void AbsorbSun()
    {
        var sunReq = _currentRequirements.GetValueOrDefault(RequirementType.sun.ToString());

        sunReq.CurrentLevel = Math.Min(sunReq.CurrentLevel + _absorptionRate, sunReq.MaxLevel);

        _logger.Info(RequirementType.sun.ToString() +
                     _currentRequirements.GetValueOrDefault(RequirementType.sun.ToString()));
    }
    
    public void Grow()
    {
        if(Stage == GrowthStage.Dead) return;
        
        AbsorbWaterFromTile();
        ConsumeWater();
        AbsorbSun();
        
        CheckRequirements();
    }

    private void DryUp()
    {
        _sprite2D.Play("Dead");
        Stage = GrowthStage.Dead;
        _logger.Info($"Plant {PlantName} has died due to lack of water.");
    }
}