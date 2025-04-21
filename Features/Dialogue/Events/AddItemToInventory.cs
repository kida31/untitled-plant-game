using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Inventory;
using untitledplantgame.Item;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class AddItemToInventory : DialogueEvent
{
	[Export] private string _itemId;
	[Export] private int _amount = 1;
	public override void Execute()
	{
		var itemStack = ItemDatabase.Instance.CreateItemStack(_itemId, _amount);
		Game.Player.Inventory.AddItem(itemStack);
	}
}
