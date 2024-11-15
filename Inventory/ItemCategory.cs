using System;

namespace untitledplantgame.Inventory;

public class ItemCategory : IEquatable<ItemCategory>
{
	public static readonly ItemCategory Plant = new("Plant");
	public static readonly ItemCategory Material = new("Material");
	public static readonly ItemCategory Medicine = new("Medicine");

	private ItemCategory(string name)
	{
		Name = name;
	}

	public string Name { get; }

	public bool Equals(ItemCategory other)
	{
		if (ReferenceEquals(null, other))
		{
			return false;
		}

		if (ReferenceEquals(this, other))
		{
			return true;
		}

		return Name == other.Name;
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

		if (obj.GetType() != GetType())
		{
			return false;
		}

		return Equals((ItemCategory)obj);
	}

	public override int GetHashCode()
	{
		return Name != null ? Name.GetHashCode() : 0;
	}

	public static bool operator ==(ItemCategory left, ItemCategory right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(ItemCategory left, ItemCategory right)
	{
		return !Equals(left, right);
	}
}
