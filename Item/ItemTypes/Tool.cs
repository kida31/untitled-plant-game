using Godot.Collections;
using untitledplantgame.Statistics;

namespace untitledplantgame.Item.ItemTypes;

public class Tool : IItemType
{
	public string GetItemStringIdentifier()
	{
		return "";
	}

	public string GetItemName()
	{
		return "";
	}

	public int GetItemStackSize()
	{
		return 0;
	}

	public Array<Stat> GetAllEntityStats()
	{
		return null;
	}
}
