using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace untitledplantgame.GUI;

public partial class UpgClickable : Control
{
	public event Action Pressed;

	public override void _Ready()
	{
		var children = GetRecursiveChildren(this);
		GD.Print("Children: ", children.Count);
		foreach (var control in children.OfType<Control>())
		{
			control.FocusMode = FocusModeEnum.None;
			control.MouseFilter = MouseFilterEnum.Ignore;
		}

		// FocusMode = FocusModeEnum.All;
		// MouseFilter = MouseFilterEnum.Pass;

		Pressed += () => GD.Print("Pressed");
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
		{
			Pressed?.Invoke();
		}

		if (@event.IsActionPressed("ui_accept"))
		{
			Pressed?.Invoke();
		}
	}

	private static List<Node> GetRecursiveChildren(Node node)
	{
		var children = new List<Node>();
		foreach (var child in node.GetChildren())
		{
			children.Add(child);
			children.AddRange(GetRecursiveChildren(child));
		}

		return children;
	}
}
