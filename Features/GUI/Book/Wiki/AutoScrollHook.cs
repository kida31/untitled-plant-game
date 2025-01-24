using System.Collections.Generic;
using Godot;

namespace untitledplantgame.GUI.Book.Wiki;

[Tool]
public partial class AutoScrollHook : Node
{
	[Export] private float _scrollDelay;
	[Export] private float _scrollSpeedPx;

	private Tween _tween;
	private ScrollTextBox _owner;

	public override string[] _GetConfigurationWarnings()
	{
		var messages = new List<string>();
		if (GetParent() is not ScrollTextBox)
		{
			messages.Add("AutoScrollHook should be a child of a ScrollContainer");
		}

		if (Owner is not ScrollTextBox)
		{
			messages.Add("AutoScrollHook should be a child of a Control");
		}

		return messages.ToArray();
	}

	public override void _Ready()
	{
		// if editor, do nothing
		if (Engine.IsEditorHint()) return;

		_owner = GetParent() as ScrollTextBox;
		if (_owner == null)
		{
			GD.PushError("AutoScrollHook should be a child of a ScrollContainer");
			QueueFree();
			return;
		}

		_owner.VisibilityChanged += OnVisibilityChanged;
		_owner.TextChanged += OnTextChanged;
	}

	private void OnTextChanged()
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
