using System;
using Godot;
using Godot.Collections;
using untitledplantgame.Plants;

public abstract partial class APlant : Node2D, IPlantable
{
    private string Nickname { get; set; }

    private GrowthStage stage { get; set; }

    private Dictionary<string, int> Requirements;

    //private APlant[] neighboringPlants; //companion planting

    private void setStage()
    {
        //check requirements to grow into next stage
        //set asset
    }

    public SoilTile tile { get; set; }

    public void PlantOnTile()
    {
        //set tile
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