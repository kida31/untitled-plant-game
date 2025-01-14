using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common.Inputs.GameActions;

namespace untitledplantgame.GUI.Components;

/// <summary>
///		A clickable control element. Similar to a button but without predefined highlight/selection features.
///		Inherit from this to extend any clickable behaviour.
/// </summary>
[Tool]
public partial class Clickable : Control, IPressable, IFocusable, ISeconaryPressable
{
	[Export] public bool Disabled = false;
	public event Action Pressed;
	public event Action SecondaryPressed;

	public Clickable()
	{
		GuiInput += OnGuiInput;
	}

	public override string[] _GetConfigurationWarnings()
	{
		var messages = new List<string>();
		if (FocusMode == FocusModeEnum.None)
		{
			messages.Add("FocusMode is set to None. Are you sure you want to do this?");
		}

		if (MouseFilter == MouseFilterEnum.Ignore)
		{
			messages.Add("MouseFilter is set to Ignore. This will ignore clicks.");
		}

		static List<Control> CollectChildren(Node node)
		{
			var children = node.GetChildren();
			var grandChildren = children.SelectMany(c => CollectChildren(c));
			return children.Concat(grandChildren).OfType<Control>().ToList();
		}

		var recursiveChildren = CollectChildren(this);
		messages.AddRange(
			recursiveChildren
				.Where(c => c.MouseFilter == MouseFilterEnum.Stop)
				.Select(c => $"Child \"{c.Name}\" is set to MouseFilter.Stop. This may block clicks on this node")
		);

		return messages.ToArray();
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (Disabled) return;

		if (@event is InputEventMouseButton button)
		{
			if (button.ButtonIndex == MouseButton.Left && button.Pressed)
			{
				Pressed?.Invoke();
			}
		}

		if (@event.IsActionPressed(UINavigation.Accept))
		{
			Pressed?.Invoke();
		}

		if (@event is InputEventMouseButton rmb)
		{
			if (rmb.ButtonIndex == MouseButton.Right && rmb.Pressed)
			{
				SecondaryPressed?.Invoke();
			}
		}
		
		if (@event.IsActionPressed(UINavigation.SecondaryAccept))
		{
			SecondaryPressed?.Invoke();
		}
	}
}
