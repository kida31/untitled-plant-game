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
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.F1)
		{
			Visible = !Visible;
		}
		
		if (@event.IsActionPressed(Base.East))
		{
			Hide();
		}
	}
}
