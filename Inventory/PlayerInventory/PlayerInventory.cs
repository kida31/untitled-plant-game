using System;
using Godot;
using untitledplantgame.Event;
using untitledplantgame.Item;

namespace untitledplantgame.Inventory.PlayerInventory;

public class PlayerInventory
{
	private untitledplantgame.Item.ItemStack[] _itemStacks;

	public PlayerInventory(int inventorySize)
	{
		_itemStacks = new untitledplantgame.Item.ItemStack[inventorySize];
		EventBus.Instance.OnItemPickUp += AddItemToInventory;
	}

	private void AddItemToInventory(untitledplantgame.Item.ItemStack item)
	{
		// TODO: Check we can actually stack on top, instead of just adding to an empty slot
		for (var i = 0; i < _itemStacks.Length; i++)
		{
			if (_itemStacks[i] == null)
			{
				_itemStacks[i] = item;
				return;
			}
		}
	}

	public void Remove(IStorable item)
	{
		var index = Array.IndexOf(_itemStacks, item);
		if (index == -1)
		{
			GD.PrintErr("Item not found in backpack.");
		}
		else
		{
			_itemStacks[index] = null;
		}
	}
}
