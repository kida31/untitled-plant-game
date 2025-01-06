using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;

public partial class DebugControlTree : Node
{
	private Logger _logger;
	public override void _Ready()
	{
		_logger = new Logger(this);
		_logger.Debug("Hooking up GuiInput events to all controls in the tree");
		
		var root = GetParent();
		var children = GetChildrenRecursive(root).OfType<Control>();
		foreach (var control in children)
		{
			control.GuiInput += (e) => OnControlGuiInput(control, e);
		}
	}

	private void OnControlGuiInput(Control control, InputEvent e)
	{
		if (e is InputEventMouseButton button)
		{
			if (button.ButtonIndex == MouseButton.Left && button.Pressed)
			{
				var path = control.GetPath().ToString();
				_logger.Debug($"Control {path.Substring(Math.Max(0, path.Length - 20))} clicked");
			}
		}
	}

	private List<Node> GetChildrenRecursive(Node root)
	{
		var children = root.GetChildren();
		var grandChildren = children.SelectMany(GetChildrenRecursive);
		return children.Concat(grandChildren).ToList();
	}
}
