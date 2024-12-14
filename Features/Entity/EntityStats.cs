using System;
using System.Linq;
using Godot;
using Godot.Collections;
using untitledplantgame.Statistics;

namespace untitledplantgame.Entity;

[Obsolete]
public partial class EntityStats : Node
{
	[Export]
	public EntityConfiguration EntityConfiguration;
	private Array<Stat> _baseStats;

	public override void _Ready()
	{
		_baseStats = new Array<Stat>();

		try
		{
			foreach (var stat in EntityConfiguration.Stats)
			{
				// Godot suffers from the same problem as Unity; Accessing Script and Scenes without them existing!
				// Modifiers CANNOT be added when the Object is created!
				Stat tempStat = new Stat(stat.GetBaseValueOfStat(), stat.CreateStatTypeInstance(stat.StatType), stat.IsHidden);
				tempStat.AddMultipleModifiers(stat.GetStatModifiers());
				_baseStats.Add(tempStat);
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr("Error in foreach loop: " + ex.Message);
		}
	}

	public Array<Stat> GetEntityStats()
	{
		return _baseStats;
	}

	// Debug Method
	private void PrintAllBaseStatsFromConfig()
	{
		foreach (var stat in _baseStats)
		{
			GD.Print(stat.GetModifiedStatValue());
		}
	}
}
