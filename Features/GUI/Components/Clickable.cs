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
	public event Action Released;
	public event Action SecondaryReleased;
	
	
	private bool _isPressedReadOnly;
	
	public bool IsPressed() => _isPressedReadOnly;

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

		if (@event is InputEventMouseButton mb)
		{
			switch (mb.ButtonIndex)
			{
				case MouseButton.Left when mb.Pressed:
					_isPressedReadOnly = true;
					Pressed?.Invoke();
					break;
				case MouseButton.Left:
					_isPressedReadOnly = false;
					Released?.Invoke();
					break;
				case MouseButton.Right when mb.Pressed:
					SecondaryPressed?.Invoke();
					break;
				case MouseButton.Right:
					SecondaryReleased?.Invoke();
					break;
			}
		}

		if (@event.IsActionPressed(UINavigation.Accept))
		{
			_isPressedReadOnly = true;
			Pressed?.Invoke();
		}
		else if (@event.IsActionReleased(UINavigation.Accept))
		{
			_isPressedReadOnly = false;
			Released?.Invoke();
		}

		if (@event.IsActionPressed(UINavigation.SecondaryAccept))
		{
			SecondaryPressed?.Invoke();
		}
		else if (@event.IsActionReleased(UINavigation.SecondaryAccept))
		{
			SecondaryReleased?.Invoke();
		}
	}
}
