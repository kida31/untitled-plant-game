using Godot;
using System;

public partial class DebugCurrentGuiFocus : Label
{
	private Viewport _viewport;

	public override void _Ready()
	{
		_viewport = GetViewport();
		if (_viewport == null)
		{
			Name = "DISABLED Could not find Viewport";
		}	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var owner = _viewport?.GuiGetFocusOwner();
		Text = "GUI Focus: " + owner?.Name ?? "No focus";
	}
}
