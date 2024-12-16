using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Item;

[GlobalClass]
public abstract partial class AComponent : Resource, ICombinable<AComponent>, IClonable<AComponent>
{
	public AComponent() {}
	
	public override string ToString()
	{
		return GetType().Name;
	}

	public abstract AComponent Clone();

	public virtual AComponent Combine(AComponent otherComponent)
	{
		return this;
	}
}
