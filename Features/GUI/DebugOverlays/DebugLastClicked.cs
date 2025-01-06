using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;

public partial class DebugLastClicked : Label
{
	private Control _lastClickedControl;

	public override void _Ready()
	{
		var root = GetTree().Root;
		var children = GetChildrenRecursive(root).OfType<Control>();

		foreach (var control in children)
		{
			control.GuiInput += (e) => OnControlGuiInput(control, e);
		}
	}

	public override void _Process(double delta)
	{
		var path = _lastClickedControl?.GetPath().ToString() ?? "";
		Text = "Last clicked: " + path.Substring(Math.Max(0, path.Length - 48));
	}

	private void OnControlGuiInput(Control control, InputEvent e)
	{
		if (e is not InputEventMouseButton {ButtonIndex: MouseButton.Left, Pressed: true})
		{
			return;
		}

		_lastClickedControl = control;
	}

	private List<Node> GetChildrenRecursive(Node root)
	{
		var children = root.GetChildren();
		var grandChildren = children.SelectMany(GetChildrenRecursive);
		return children.Concat(grandChildren).ToList();
	}
}
