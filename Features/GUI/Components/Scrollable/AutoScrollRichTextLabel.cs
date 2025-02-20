﻿using System.Collections.Generic;
using Godot;

namespace untitledplantgame.GUI.Components.Scrollable;

/// <summary>
///		A control for displaying text that also automatically scrolls.
/// </summary>
[Tool]
public partial class AutoScrollRichTextLabel : RichTextLabel
{
	/// <summary>
	///		 The delay in seconds before the scrolling starts.
	/// </summary>
	[Export(PropertyHint.Range, "0.0,2.0")] private float _scrollDelay;

	/// <summary>
	///		 The speed of the scrolling in pixels per second.
	/// </summary>
	[Export(PropertyHint.Range, "0,50")] private float _scrollSpeedPps;

	private Tween _tween;

	public override string[] _GetConfigurationWarnings()
	{
		var messages = new List<string>();
		if (_scrollSpeedPps <= 0)
		{
			messages.Add("ScrollSpeedPps should be greater than 0");
		}

		if (FitContent)
		{
			messages.Add("FitContent should be disabled");
		}

		if (ScrollActive)
		{
			messages.Add("It is recommended to disable ScrollActive");
		}

		return messages.ToArray();
	}

	public override void _Ready()
	{
		// if editor, do nothing
		if (Engine.IsEditorHint()) return;

		VisibilityChanged += StartScrolling;
		Finished += StartScrolling;
	}

	private void StartScrolling()
	{
		ScrollToLine(0);

		var scrollBar = GetVScrollBar();
		var yMax = scrollBar.MaxValue;
		_tween?.Stop();
		_tween = CreateTween();
		_tween.TweenMethod(Callable.From<double>(scrollBar.SetValue), 0, yMax, yMax / _scrollSpeedPps)
			.SetDelay(_scrollDelay);
	}
}
