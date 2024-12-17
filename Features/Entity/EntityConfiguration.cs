using System.Linq;
using Godot;
using Godot.Collections;
using untitledplantgame.Item;
using untitledplantgame.Statistics;

namespace untitledplantgame.Entity;

[GlobalClass]
public partial class EntityConfiguration :  AComponent
{
	[Export]
	public string Notes { get; set; }

	[Export]
	public Array<Stat> Stats { get; set; } = new();
	
	public EntityConfiguration()
		: this(null) { }

	public EntityConfiguration(string note)
	{
		Notes = note;
	}

	public void PrintOutAllModifiedStats()
	{
		foreach (var aStat in Stats)
		{
			GD.Print(aStat.GetModifiedStatValue());
		}
	}

	public override AComponent Clone()
	{
		// TODO: Is this correct?
		var newConfig = new EntityConfiguration(Notes);
		foreach (var stat in Stats)
		{
			newConfig.Stats.Add(new Stat(stat.GetModifiedStatValue(), stat.StatType, stat.IsHidden));
		}
		return newConfig;
	}

	public override AComponent Combine(AComponent component)
	{
		if (component is not EntityConfiguration entityConfig)
		{
			return this;
		}

		var newConfig = (EntityConfiguration) Clone();
		foreach (var statInEntityConfig in entityConfig.Stats)
		{
			var matchingStat = newConfig.Stats.FirstOrDefault(stat => stat.StatType.GetType() == statInEntityConfig.StatType.GetType());
				
			if (matchingStat != null)
			{
				// AddModifier if stat exists
				matchingStat.AddStatModifier(statInEntityConfig.GetModifiedStatValue());
			}
			else
			{
				// Add new stat if it doesn't exist
				newConfig.Stats.Add(new Stat(statInEntityConfig.GetModifiedStatValue(), statInEntityConfig.StatType, false));
			}
		}

		// If the new EntityConfiguration isn't valid, we can just return the same without changes.
		return newConfig;
	}
}
