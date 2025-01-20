using System;
using System.Linq;
using Godot;
using untitledplantgame.GUI.Items;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class CraftingInventory : StorageView
{
	public Action<IItemStack> RemovingItemFromInventory;
	
	public override void _Ready()
	{
		base._Ready();
		ItemViews.ForEach(i => i.Pressed += () => OnItemViewPressed(i));
	}

	private void OnItemViewPressed(NewInventoryItemView newInventoryItemView)
	{
		ItemViews[newInventoryItemView.SlotIndex].ItemStack.Amount--;
		var item = newInventoryItemView.ItemStack.Clone();
		item.Amount = 1;
		
		RemovingItemFromInventory?.Invoke(item);
	}

	public void AddItem(IItemStack item)
	{
		//if already in inventory and max stack size not reached, add to existing stack	
		foreach (var view in ItemViews.Where(view => view.ItemStack == item && view.ItemStack.Amount < view.ItemStack.MaxStackSize))
		{
			view.ItemStack.Amount += item.Amount;
			return;
		}
		
		//else insert into first empty slot
		foreach (var view in ItemViews.Where(view => view.ItemStack == null))
		{
			view.Inventory.AddItem(item);
			break;
		}
	}
}
