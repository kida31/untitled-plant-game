using Godot;
using System;
using System.Collections.Generic;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Item;

public partial class NotificationDemo : Control
{
	private List<IItemStack> items;
	public override void _Ready()
	{
		items = ItemDatabase.Instance.GetAllItems();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			var i = (int) (GD.Randi() % items.Count);
			var randomItem = items[i];
			EventBus.Instance.ItemAddedToInventory(randomItem);
		}
	}
}
