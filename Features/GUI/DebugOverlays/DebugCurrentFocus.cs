using Godot;

public partial class DebugCurrentFocus : Label
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

	public override void _Process(double delta)
	{
		var owner = _viewport?.GuiGetFocusOwner();
		Text = "CurrentFocus: " + owner?.Name;
	}
}
