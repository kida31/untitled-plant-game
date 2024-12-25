using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Medicine;

namespace untitledplantgame.Item.Components;

public partial class MedicineComponent : AComponent
{
	public Dictionary<MedicinalEffect, int> TheGoodStuff;
	public Dictionary<IllnessEffect, int> TheBadStuff;

	public MedicineComponent(Dictionary<MedicinalEffect, int> theGoodStuff, Dictionary<IllnessEffect, int> theBadStuff)
	{
		TheGoodStuff = theGoodStuff;
		TheBadStuff = theBadStuff;
	}
	
	//used for processing 
	public override AComponent Combine(AComponent otherComponent)
	{
		if (otherComponent is not MedicineComponent component)
		{
			throw new InvalidOperationException("Cannot combine MedicinalComponent with other component type");
		}

		foreach (var (effect, value) in component.TheGoodStuff)
		{
			if (!TheGoodStuff.ContainsKey(effect))
			{
				continue;
			}

			TheGoodStuff[effect] += value;
			if (TheGoodStuff[effect] <= 0)
			{
				TheGoodStuff.Remove(effect);
			}
		}
		
		foreach (var (effect, value) in component.TheBadStuff)
		{
			if (!TheBadStuff.ContainsKey(effect))
			{
				continue;
			}

			TheBadStuff[effect] += value;
			if (TheBadStuff[effect] <= 0)
			{
				TheBadStuff.Remove(effect);
			}
		}

		return this;
	}

	//used for mixing several plants
	public MedicineComponent Mix(MedicineComponent component)
	{
		var newGoodStuff = new Dictionary<MedicinalEffect, int>(TheGoodStuff);
		var newBadStuff = new Dictionary<IllnessEffect, int>(TheBadStuff);

		foreach (var (effect, value) in component.TheGoodStuff)
		{
			if (!newGoodStuff.TryAdd(effect, value))
			{
				newGoodStuff[effect] += value;
			}
		}
		
		foreach (var (effect, value) in component.TheBadStuff)
		{
			if (!newBadStuff.TryAdd(effect, value))
			{
				newBadStuff[effect] += value;
			}
		}

		return new MedicineComponent(newGoodStuff, newBadStuff);
	}

	public override bool Equals(AComponent other)
	{
		return other is MedicineComponent;
	}

	public override MedicineComponent Clone() => new(TheGoodStuff, TheBadStuff);

	public override string ToString()
	{
		var result = "";
		foreach (var VARIABLE in TheGoodStuff)
		{
			result += $" {VARIABLE.Key} : {VARIABLE.Value}";
		}

		return "MedicinalComponent with effects: " + result;
	}
}
