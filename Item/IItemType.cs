using Godot.Collections;
using untitledplantgame.Statistics;

namespace untitledplantgame.Item;

public interface IItemType
{
	string GetItemStringIdentifier();
	string GetItemName();
	int GetItemStackSize();
	Array<Stat> GetAllEntityStats();
}
