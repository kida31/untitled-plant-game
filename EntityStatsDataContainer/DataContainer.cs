using Godot;
using Godot.Collections;
using untitledplantgame.Entity;
using untitledplantgame.Statistics;

namespace untitledplantgame.EntityStatsDataContainer;

public partial class DataContainer : Node
{
	[Export]
	private EntityStats _entityStats;

	// Bad idea, but okay for now
	public Array<Stat> GetEntityBaseStats()
	{
		return _entityStats.GetEntityStats();
	}
}
