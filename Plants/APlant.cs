using System;
using Godot;
using untitledplantgame.Plants;

public abstract class APlant : Node2D, IPlantable
{
    private string Nickname { get; set; }

    private GrowthStage stage { get; set; }

    private void setStage()
    {
        //check requirements to grow into next stage
        //set asset
    }

    public SoilTile tile { get; set; }

    public void PlantOnTile()
    {
        throw new NotImplementedException();
    }

    public void Hydrate()
    {
        throw new NotImplementedException();
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