using Godot;
using System;
using untitledplantgame.Common.Inputs.GameActions;

public partial class ControllerLayout : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.F1))
		{
			Visible = !Visible;
		}

		if (Input.IsActionPressed(Base.East))
		{
			Hide();
		}
	}
}
