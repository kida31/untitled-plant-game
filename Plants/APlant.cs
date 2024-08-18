using System;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace untitledplantgame.Plants;
public partial class APlant : Node2D
{
    private string Nickname { get; set; }

    private string PlantName { get; set; }

    private int _plantId;

    private float _absorptionRate = 10.0f;

    [Export] public GrowthStage Stage { get; private set; } = GrowthStage.Seedling;

    private Dictionary<string, Requirement> _currentRequirements;

    public override void _Ready()
    {
        UpdateRequirements();
    }

    public APlant()
    {
    }

    public APlant(int plantId,GrowthStage stage)
    {
        _plantId = plantId;
        Stage = stage;
    }
    
    void UpdateRequirements()
    {
        _currentRequirements = MagicConch.Instance.GetRequirements(_plantId, Stage);
    }
    
    public void CheckRequirements()
    {
        GD.Print("Water Level: " + _currentRequirements.GetValueOrDefault("water"));
        GD.Print("Sun Level: " + _currentRequirements.GetValueOrDefault("sun"));
        
        var fulfilled = _currentRequirements.All(req => req.Value.IsFulfilled());

        if (!fulfilled || Stage == GrowthStage.Ripening) return;

        Stage += 1;
        UpdateRequirements();
    }

    public SoilTile Tile { get; set; }

    public void PlantOnTile(SoilTile soilTile)
    {
        Tile = soilTile;
    }

    public void AbsorbWaterFromTile()
    {
        var waterAbsorbed = Tile.WithdrawHydration(_absorptionRate);
        _currentRequirements.GetValueOrDefault("water").CurrentLevel += waterAbsorbed;
        GD.Print(_currentRequirements.GetValueOrDefault("water"));
    }

    public void AbsorbSun(float sunLevel)
    {
        _currentRequirements.GetValueOrDefault("sun").CurrentLevel += sunLevel;
        GD.Print(_currentRequirements.GetValueOrDefault("sun"));
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