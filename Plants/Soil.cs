using Godot;
using System;
using untitledplantgame.Plants;

public partial class Soil : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var button = GetNode<Button>("Button");
		button = GetNode("Button") as Button;

		button.Pressed += () => GD.Print("Hello World");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
