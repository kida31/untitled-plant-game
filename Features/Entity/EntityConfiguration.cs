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

	public override AComponent CombineComponent(AComponent otherComponent)
	{
		if (otherComponent.GetType() == GetType())
		{
			var entityConfig = otherComponent as EntityConfiguration;
			
			foreach (var statInEntityConfig in entityConfig.Stats)
			{
				var matchingStat = Stats.FirstOrDefault(stat => stat.StatType.GetType() == statInEntityConfig.StatType.GetType());
				
				if (matchingStat != null)
				{
					// AddModifier if stat exists
					matchingStat.AddStatModifier(statInEntityConfig.GetModifiedStatValue());
				}
				else
				{
					// Add new stat if it doesn't exist
					Stats.Add(new Stat(statInEntityConfig.GetModifiedStatValue(), statInEntityConfig.StatType, false));
				}
			}
		}

		// If the new EntityConfiguration isn't valid, we can just return the same without changes.
		return this;
	}
}
