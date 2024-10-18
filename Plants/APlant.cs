using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
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
}

public partial class APlant : Node2D, IPlantable
{
    public string PlantName { get; private set; }
    [Export] private int _plantId;

    private float _absorptionRate = 50.0f;

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

    public void CheckRequirements()
    {
        var fulfilled = _currentRequirements.All(req => req.Value.IsFullfilled());

        if (!fulfilled || Stage == GrowthStage.Ripening) return;

        _currentDay++;
        _logger.Info($"Checking Requirement for stage {Stage}, current day count at {_currentDay} of {_daysToGrow}.");
        AdvanceStage();
    }

    private void AdvanceStage()
    {
        if (_currentDay < _daysToGrow) return;

        Stage++;
        _logger.Info($"Advancing stage to {Stage}.");
        UpdateRequirements();
    }

    public void PlantOnTile(SoilTile soilTile)
    {
        Tile = soilTile;
    }

    public void AbsorbWaterFromTile()
    {
        var waterReq = _currentRequirements.GetValueOrDefault(RequirementType.water.ToString());
        var waterAbsorbed = Tile.WithdrawHydration(_absorptionRate) + waterReq.CurrentLevel;

        waterReq.CurrentLevel = Math.Min(waterAbsorbed, waterReq.MaxLevel);

        _logger.Info(_currentRequirements.GetValueOrDefault(RequirementType.water.ToString()).ToString());
    }

    public void AbsorbSun()
    {
        var sunReq = _currentRequirements.GetValueOrDefault(RequirementType.sun.ToString());

        sunReq.CurrentLevel = Math.Min(sunReq.CurrentLevel + _absorptionRate, sunReq.MaxLevel);

        _logger.Info(_currentRequirements.GetValueOrDefault(RequirementType.sun.ToString()).ToString());
    }
}