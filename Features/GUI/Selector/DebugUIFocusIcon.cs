using Godot;
using untitledplantgame.GUI.Selector;

/// <summary>
///     Displays the position and size of the target node. Used for debugging.
/// </summary>
public partial class DebugUIFocusIcon : Label
{
	[Export] private UIFocusIcon _scriptOwner;
	[Export] private Control _readPosFrom;
	[Export] private Control _readSizeFrom;

	public override void _Process(double delta)
	{
		// Parent object
		var size = Vector2.Zero;
		var pos = Vector2.Zero;
		
		// Targeted object (focused control)
		var tSize = Vector2.Zero;
		var tPos = Vector2.Zero;

		if (IsInstanceValid(_readPosFrom))
		{
			pos = _readPosFrom.GetGlobalRect().GetCenter();
		}

		if (IsInstanceValid(_readSizeFrom))
		{
			size = _readSizeFrom.Size;
		}

		if (IsInstanceValid(_scriptOwner.FocusedControl))
		{
			var tRect = _scriptOwner.FocusedControl.GetGlobalRect();
			tPos = tRect.GetCenter();
			tSize = tRect.Size;
		}

		Text = $"Position: {pos.X:F1}x{pos.Y:F1}";
		Text += $"\nTargetPos: {tPos.X:F1}x{tPos.Y:F1}";
		Text += $"\nSize: {size.X:F1}x{size.Y:F1}";
		Text += $"\nTargetSize: {tSize.X:F1}x{tSize.Y:F1}";
	}
}
