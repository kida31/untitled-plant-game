using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using untitledplantgame.Inventory.GeneralInventory;


public partial class GeneralInventoryLogic : Node
{
	private Array<Node> Items;
	private List<ICategoryTab> _categories;
	
	//logic bekommt item. which category? ok go this one
	// hat doch alle items und categorized sie nur zur anzeige (filter mässig)
	
	/*
	 * GDD:
	 * drei tabs (seeds, herbs/harvested, medicine/crafted)
	 * click Inventory Button (I) to open
	 * switch between tabs
	 * auto-arrange
	 * drag and drop
	 * close inventory (maybe same (I))
	 * stackable
	 * Items as objects
	 */
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Items = this.GetChildren();
		GD.Print(Items);
	}
}
