using System;
using Godot;
using System.Collections.Generic;

namespace untitledplantgame.Plants;
public abstract partial class Seedling : Node2D, IPlantable
{
    private string Nickname { get; set; }

    private Dictionary<string, Requirement> requirements;

    private void setStage(GrowthStage growthStage)
    {
        //set asset
        stage = growthStage;
    }

    void checkRequirements()
    {
        //nachts aufgerufen
        //if requirements all at max
        //setStage(stage.Next());
    }

    public SoilTile tile { get; set; }

    public void PlantOnTile(SoilTile soilTile)
    {
        //set tile
        tile = soilTile;
        setStage(GrowthStage.Seedling); //first stage is set
    }

    public void Hydrate()
    {
        //check tile
        //get hydration from tile
        //
        //SoilTile.reduceHydration(amount);
    }
}

enum GrowthStage
{
    Seedling,
    Vegetating,
    Budding,
    Flowering,
    Ripening,
}