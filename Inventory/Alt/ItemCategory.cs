﻿using System;

namespace untitledplantgame.Inventory.Alt;

public class ItemCategory : IEquatable<ItemCategory>
{
	public static ItemCategory Plant = new("Plant");
	public static ItemCategory Material = new("Material");
	public static ItemCategory Medicine = new("Medicine");

	public string Name { get; }

	private ItemCategory(string name)
	{
		Name = name;
	}

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

		return Equals((ItemCategory) obj);
	}

	public override int GetHashCode()
	{
		return Name != null ? Name.GetHashCode() : 0;
	}
}
