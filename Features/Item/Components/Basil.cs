using System;
using Godot;

namespace untitledplantgame.Item.Components;

public partial class Basil : AComponent, IEquatable<Basil>
{
	public Basil() {} //needed to instantiate the class

	public bool Equals(Basil other)
	{
		return other != null;
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

		return Equals((Basil) obj);
	}

	public override int GetHashCode()
	{
		return (int) "BasilComponent".Hash();
	}

	public override AComponent Clone()
	{
		return new Basil();
	}
}
