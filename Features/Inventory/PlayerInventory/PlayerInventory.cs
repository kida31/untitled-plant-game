using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory.PlayerInventory;

public class PlayerInventory
{
	private IItemStack[] _itemStacks;
	// public  ItemStack... Hand;
	
	
	public PlayerInventory(int inventorySize)
	{
		_itemStacks = new ItemStack[inventorySize];
		EventBus.Instance.OnItemPickUp += AddItemToInventory;
		/// += GetitemInHand;
	}

	private void AddItemToInventory(IItemStack item)
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

	
	//public Tool GetItemInHand() { return Hand; } 
}
