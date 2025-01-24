using System.Collections.Generic;
using Godot;

namespace untitledplantgame.GUI.Components.Scrollable;

/// <summary>
///		Automatically scrolls the parent ScrollLabel whenever the content changed or the parent becomes visible.
/// </summary>
[Tool]
public partial class AutoScroll : Node
{
	[Export] private float _scrollDelay;
	[Export] private float _scrollSpeedPx;

	private Tween _tween;
	private ScrollLabel _owner;

	public override string[] _GetConfigurationWarnings()
	{
		var messages = new List<string>();
		if (GetParent() is not ScrollLabel)
		{
			messages.Add("AutoScrollHook should be a child of a ScrollLabel");
		}

		return messages.ToArray();
	}

	public override void _Ready()
	{
		// if editor, do nothing
		if (Engine.IsEditorHint()) return;

		_owner = GetParent() as ScrollLabel;
		if (_owner == null)
		{
			GD.PushError("AutoScroll should be a child of a ScrollLabel");
			QueueFree();
			return;
		}

		_owner.VisibilityChanged += OnVisibilityChanged;
		_owner.GetVScrollBar().Changed += OnContentChanged;
	}

	private void OnContentChanged()
	{
		StartScrolling();
	}

	private void OnVisibilityChanged()
	{
		if (!_owner.IsVisibleInTree()) return;
		StartScrolling();
	}

	private void StartScrolling()
	{
		_tween?.Stop();
		_tween = CreateTween();

		var yMax = _owner.GetVScrollBar().MaxValue;
		SetYScroll(0);
		_tween.TweenMethod(Callable.From<float>(SetYScroll), 0, 0, 0).SetDelay(_scrollDelay);
		_tween.TweenMethod(Callable.From<float>(SetYScroll), 0, yMax, yMax / _scrollSpeedPx);
	}
	
	private void SetYScroll(float value)
	{
		_owner.GetVScrollBar().Value = value;
	}
}
