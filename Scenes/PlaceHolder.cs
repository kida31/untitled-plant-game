using Godot;
using System;
using untitledplantgame.Common;

public partial class PlaceHolder : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup(GameGroup.Placeholder);
	}

}
