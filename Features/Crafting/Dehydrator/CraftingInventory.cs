﻿using System;
using System.Linq;
using Godot;
using untitledplantgame.GUI.Items;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class CraftingInventory : StorageView
{
	public Action<NewInventoryItemView> RemovingItemFromInventory;
	
	public override void _Ready()
	{
		base._Ready();
		ItemViews.ForEach(i => i.Pressed += () => OnItemViewPressed(i));
	}

	private void OnItemViewPressed(NewInventoryItemView newInventoryItemView)
	{
		RemovingItemFromInventory?.Invoke(newInventoryItemView);
	}
}
