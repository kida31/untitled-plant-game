using System;
using System.Collections.Generic;
using untitledplantgame.Medicine;

namespace untitledplantgame.Item.Components;

public partial class IllnessComponent : AComponent
{
	public Dictionary<Illness,int> Illness;
	public IllnessComponent(Dictionary<Illness,int> illness)
	{
		Illness = illness;
	}
	
	public override AComponent Combine(AComponent otherComponent)
	{
		if (otherComponent is not IllnessComponent component)
		{
			throw new InvalidOperationException("Cannot combine MedicinalComponent with other component type");
		}

		foreach (var (effect, value) in component.Illness)
		{
			if (Illness.ContainsKey(effect))
			{
				Illness[effect] += value;
			}
		}

		return this;
	}
	
	public override AComponent Clone() => new IllnessComponent(Illness);
}
