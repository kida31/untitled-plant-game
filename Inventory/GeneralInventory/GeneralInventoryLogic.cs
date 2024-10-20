using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using untitledplantgame.Inventory.GeneralInventory;


public partial class GeneralInventoryLogic : Node
{
	private Array<Node> Items;
	private List<ICategoryTab> _categories;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
