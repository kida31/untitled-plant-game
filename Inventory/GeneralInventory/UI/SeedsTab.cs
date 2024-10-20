using Godot;
using System;
using Godot.Collections;
using untitledplantgame.Inventory.GeneralInventory;

public partial class SeedsTab : Node, ICategoryTab
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public string GetICategoryTabName()
	{
		return "Seed";
	}

	public Array<Node> GetItemsInCategoryTab()
	{
		throw new NotImplementedException();
	}
}
