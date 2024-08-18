using System;
using Godot;
using untitledplantgame.Plants;

// TODO Test ALL APlant public methods and mechanics

public partial class TestAPlant : Node2D
{
	private APlant plant;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Assign (Setup)
		var plant = new APlant(123, GrowthStage.Seedling);
		AddChild(plant);
		// NOT FULFILLED REQUIREMEN TS
		
		// Action (Trigger Functionality)
		plant.CheckRequirements(); // Should NOT stage + 1
		
		// Assert (Check if functionality was successful)
		if (plant.Stage != GrowthStage.Seedling)
		{
			throw new InvalidOperationException("Stage was modified, but should not have");
		}
		
		var plant1 = new APlant(123, GrowthStage.Seedling);
		AddChild(plant1);
		plant1.AbsorbSun(100);
		plant1.CheckRequirements();

		if (plant1.Stage != GrowthStage.Vegetating)
		{
			throw new InvalidOperationException("Plant did not advance in stage");
		}
		GD.Print("Plant1 on stage " + plant1.Stage);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
