using Godot;
using System;

public partial class Main : Node

{
	[Export] private SubViewport _subViewport;

	public override void _Input(InputEvent @event)
	{
		_subViewport.PushInput(@event);
	}
}
