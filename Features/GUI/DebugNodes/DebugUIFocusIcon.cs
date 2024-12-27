using Godot;
using untitledplantgame.GUI;

public partial class DebugUIFocusIcon : Label
{
	[Export] private UIFocusIcon _scriptOwner;
	[Export] private Control _readPosFrom;
	[Export] private Control _readSizeFrom;

	public override void _Process(double delta)
	{
		var pos = _readPosFrom?.GetGlobalRect().GetCenter() ?? Vector2.Zero;
		var size = _readSizeFrom.Size;
		var tRect = _scriptOwner.FocusedControl?.GetGlobalRect();
		var tPos = tRect?.GetCenter() ?? Vector2.Zero;
		var tSize = tRect?.Size ?? Vector2.Zero;

		Text = $"Position: {pos.X:F1}x{pos.Y:F1}";
		Text += $"\nTargetPos: {tPos.X:F1}x{tPos.Y:F1}";
		Text += $"\nSize: {size.X:F1}x{size.Y:F1}";
		Text += $"\nTargetSize: {tSize.X:F1}x{tSize.Y:F1}";
	}
}
