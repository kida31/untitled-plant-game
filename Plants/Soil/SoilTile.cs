using System;
using Godot;

namespace untitledplantgame.Plants;

public partial class SoilTile : Node2D
{
    [Export] public float Hydration { get; private set; }

    private float Fertilization { get; set; }

    public float WithdrawHydration(float reductionValue)
    {
        var prevHydration = Hydration;
        Hydration = Math.Clamp(Hydration - reductionValue, 0, Hydration);

        return prevHydration - Hydration;
    }

    public void WaterTile(float addedWater)
    {
        Hydration += addedWater;
    }

    public void EvaporateWater()
    {
        Hydration--;
    }
}