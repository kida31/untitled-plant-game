using Godot;
using Godot.Collections;
using untitledplantgame.Entity;
using untitledplantgame.Statistics;

namespace untitledplantgame.Item.ItemTypes;

public class SeedCategory : IItemCategory
{
	private EntityStats _entityStats;
	
	private readonly string _itemId;
	private readonly string _displayName;
	private readonly int _stackSize;
	
	public SeedCategory(string pathToStatResource, string itemId, string displayName, int stackSize)
	{
		_entityStats = EntityStats.FromFile(pathToStatResource);
		
		_itemId = itemId;
		_displayName = displayName;
		_stackSize = stackSize;
	}
	
	public string GetItemStringIdentifier()
	{
		return _itemId;
	}
	
	public string GetItemName()
	{
		return _displayName;
	}

	public int GetItemStackSize()
	{
		return _stackSize;
	}
	
	public Array<Stat> GetAllEntityStats()
	{
		return _entityStats.BaseStats;
	}
}
