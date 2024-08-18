using System;
using Godot;
using System.Collections.Generic;

namespace untitledplantgame.Plants;
public partial class APlant : Node2D
{
    private string Nickname { get; set; }

    private string PlantName { get; set; }

    private int _plantId;
    public GrowthStage Stage { get; private set; }

    private Dictionary<string, Requirement> _currentRequirements;

    public APlant(int plantId,GrowthStage stage)
    {
        _plantId = plantId;
        Stage = stage;
        _currentRequirements = MagicConch.Instance.GetRequirements(plantId, stage);
    }
    
    public void UpdateRequirements()
    {
       // TheMagicConch.GetRequirements();
    }
    
    public void CheckRequirements()
    {
        //nachts aufgerufen
        bool allFullfilled = true;
        foreach (var req in _currentRequirements)
        {
            if (!req.Value.isFullfilled())
            {
                allFullfilled = false;
                break;
            }
        }

        if (allFullfilled && Stage != GrowthStage.Ripening)
        {
            Stage += 1;
        }
    }

    public SoilTile Tile { get; set; }

    public void PlantOnTile(SoilTile soilTile)
    {
        //set tile
        Tile = soilTile;
    }

    public void Hydrate()
    {
        //check tile
        //get hydration from tile
        //
        //SoilTile.reduceHydration(amount);
    }

    public void AbsorbSun(float SunLevel)
    {
        _currentRequirements.GetValueOrDefault("sun").currentLevel += SunLevel;
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