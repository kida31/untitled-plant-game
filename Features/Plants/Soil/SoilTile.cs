using System;
using Godot;

namespace untitledplantgame.Plants.Soil;

public partial class SoilTile : Area2D, IWaterable
{
	[Export]
	public float Hydration { get; private set; }
	private float _maxHydration = 200;
	private float Fertilization { get; set; }

	public float WithdrawHydration(float reductionValue)
	{
		var prevHydration = Hydration;
		Hydration = Math.Clamp(Hydration - reductionValue, 0, Hydration);

		return prevHydration - Hydration;
	}

	public void AddWater(float addedWater)
	{
		Hydration = Math.Min(Hydration + addedWater, _maxHydration);
	}

	//Do we want this?
	public void EvaporateWater()
	{
		Hydration--;
	}
}
