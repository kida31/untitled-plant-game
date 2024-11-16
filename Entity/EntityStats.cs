using System;
using Godot;
using Godot.Collections;
using untitledplantgame.Statistics;

namespace untitledplantgame.Entity;

public class EntityStats
{
	[Export]
	private Node _rootNode;
	private Array<Stat> _baseStats;

	public EntityStats(EntityConfiguration entityConfiguration)
	{
		_baseStats = new Array<Stat>();
		
		try
		{
			foreach (var stat in entityConfiguration.Stats)
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

	public Array<Stat> GetBaseStats()
	{
		// Ofc I need all the get stat and stuff, but this is as good as it gets for now.
		return _baseStats;
	}

	
	
	// Debug Method
	public void PrintAllBaseStatsFromConfig()
	{
		foreach (var stat in _baseStats)
		{
			GD.Print(stat.GetModifiedStatValue());
		}
	}
}
