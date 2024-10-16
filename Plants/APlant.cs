using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.MagicBoxForData;

namespace untitledplantgame.Plants;

public partial class APlant : Node2D, IPlantable
{
    public string PlantName { get; private set; }

    private int _plantId;
    
    private float _absorptionRate = 50.0f;

    private AnimatedSprite2D _sprite2D;
    public SoilTile Tile { get; set; }

    [Export] public GrowthStage Stage { get; private set; } = GrowthStage.Seedling;

    [Export] public Dictionary<GrowthStage, int> Stages;

    private Dictionary<string, Requirement> _currentRequirements;

    private int _daysToGrow;
    private int _currentDay;

    public APlant()
    {
    }

    public APlant(int plantId, string name, GrowthStage stage)
    {
        _plantId = plantId;
        PlantName = name;
        Stage = stage;
    }

    public override void _Ready()
    {
        _sprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        UpdateRequirements();
    }

    void UpdateRequirements()
    {
        var plantData = ResourceManager.Instance.GetPlantData(_plantId, Stage);
        var plantRequirements = new Dictionary<string, Requirement>();
        
        var plantDataRequirementsForStage = plantData.DataForGrowthStages[(int)Stage].GrowthRequirements;

        foreach (var data in plantDataRequirementsForStage)
        {
            plantRequirements[data.Name.ToString()] = new Requirement(data.MaxLevel, data.MinLevel);
        }

        _daysToGrow = plantData.DataForGrowthStages[(int) Stage].DaysToGrow;
        _currentRequirements = plantRequirements;
        
        _sprite2D.Play(Stage.ToString());
    }

    public void CheckRequirements()
    {
        var fulfilled = _currentRequirements.All(req => req.Value.CanAdvanceStage());

        if (!fulfilled || Stage == GrowthStage.Ripening) return;
        
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
        
        GD.Print(_currentRequirements.GetValueOrDefault(RequirementType.water.ToString()));
    }

    public void AbsorbSun()
    {
        var sunReq = _currentRequirements.GetValueOrDefault(RequirementType.sun.ToString());
        
        sunReq.CurrentLevel = Math.Min(sunReq.CurrentLevel + _absorptionRate, sunReq.MaxLevel);
        
        GD.Print(_currentRequirements.GetValueOrDefault(RequirementType.sun.ToString()));
    }
}

public enum GrowthStage
{
    Seedling,
    Vegetating,
    Budding,
    Flowering,
    Ripening,
}