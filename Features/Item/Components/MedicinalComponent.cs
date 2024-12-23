using System;
using System.Collections.Generic;
using untitledplantgame.Medicine;

namespace untitledplantgame.Item.Components;

public partial class MedicinalComponent : AComponent
{
	public Dictionary<MedicinalEffect, int> Effect;

	public MedicinalComponent(Dictionary<MedicinalEffect, int> effect)
	{
		Effect = effect;
	}

	public override AComponent Combine(AComponent otherComponent)
	{
		if (otherComponent is not MedicinalComponent component)
		{
			throw new InvalidOperationException("Cannot combine MedicinalComponent with other component type");
		}

		foreach (var (effect, value) in component.Effect)
		{
			if (Effect.ContainsKey(effect))
			{
				Effect[effect] += value;
			}
		}

		return this;
	}

	public override bool Equals(AComponent other)
	{
		return other is MedicinalComponent component && Effect == component.Effect;
	}

	public override MedicinalComponent Clone() => new (Effect);
}
