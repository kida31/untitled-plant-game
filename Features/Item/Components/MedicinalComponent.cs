using Godot.Collections;
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
		if (otherComponent is MedicinalComponent component)
		{
			otherComponent = component;
		}
		return base.Combine(otherComponent);
	}

	public override bool Equals(AComponent other)
	{
		return other is MedicinalComponent component && Effect == component.Effect;
	}

	public override MedicinalComponent Clone() => new MedicinalComponent(Effect);
}
