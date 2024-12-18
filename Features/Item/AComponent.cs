using System;
using Godot;
using untitledplantgame.Inventory;
using untitledplantgame.Item.Components;

namespace untitledplantgame.Item;

[GlobalClass]
public abstract partial class AComponent : Resource, ICombinable<AComponent>, IClonable<AComponent>, IEquatable<AComponent>
{
	public AComponent() { }

	public override string ToString()
	{
		return GetType().Name;
	}

	public abstract AComponent Clone();

	public virtual AComponent Combine(AComponent otherComponent)
	{
		return this;
	}

	public virtual bool Equals(AComponent other)
	{
		return other != null && GetType() == other.GetType();
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj))
		{
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (obj.GetType() != this.GetType())
		{
			return false;
		}

		return Equals((AComponent)obj);
	}

	public override int GetHashCode()
	{
		return GetType().GetHashCode();
	}
}
