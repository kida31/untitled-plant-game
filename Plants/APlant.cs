using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.MagicBoxForData;

namespace untitledplantgame.Plants;
public partial class APlant : Node2D
{
    private string Nickname { get; set; }

    private string PlantName { get; set; }

    private int _plantId;

    private float _absorptionRate = 50.0f;

    private AnimatedSprite2D _sprite2D;

    [Export] public GrowthStage Stage { get; private set; } = GrowthStage.Seedling;

    private Dictionary<string, Requirement> _currentRequirements;

    public override void _Ready()
    {
        _sprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
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
        _currentRequirements = ResourceManager.Instance.GetRequirements(0, Stage);
        _sprite2D.Play(Stage.ToString());
    }
    
    public void CheckRequirements()
    {
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

    public void AbsorbSun()
    {
        _currentRequirements.GetValueOrDefault("sun").CurrentLevel += _absorptionRate;
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