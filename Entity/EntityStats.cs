using System;
using System.Linq;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Statistics;

namespace untitledplantgame.Entity;

[GlobalClass]
public partial class EntityStats : Resource, IComponent
{
	[Export] public Array<Stat> BaseStats { get; set; }

	public EntityStats()
	{
		BaseStats = new();
	}

	public EntityStats(Array<Stat> baseStats)
	{
		BaseStats = baseStats;
	}

	public static EntityStats FromFile(string path)
	{
		return GD.Load<EntityStats>(path);
	}
}
