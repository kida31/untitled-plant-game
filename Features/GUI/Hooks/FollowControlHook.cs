﻿using System.Collections.Generic;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.GUI.Hooks;

/// <summary>
///		This hook makes parent Control node follow the targeted control node.
/// </summary>
public partial class FollowControlHook : Node
{
	public enum PositionMode
	{
		/// <summary>
		/// <para>Anchor the node to the cursor.</para>
		/// </summary>
		Origin,

		/// <summary>
		/// <para>Center the node on the cursor.</para>
		/// </summary>
		Center
	}

	/// <summary>
	/// <para>The node positioning behavior. Whether the origin or center position of target should be moved.</para>
	/// </summary>
	[Export] public PositionMode PositionModeSelf = PositionMode.Center;

	/// <summary>
	/// <para>The node positioning behavior. Whether the origin or center position of focused object should be used.</para>
	/// </summary>
	[Export] public PositionMode PositionModeTarget = PositionMode.Center;

	/// <summary>
	/// <para>The smoothing factor for the position change. Values small or equal to 0 make changes instant</para>
	/// </summary>
	[Export] public float SmoothingFactor = 10.0f;

	[Export]
	public Control Target
	{
		get => _target;
		set => SetTarget(value);
	}

	private Control _target;
	private Control _self;
	private Logger _logger;

	private LinkedList<Control> _targetHistory = new();

	public override void _Ready()
	{
		_self = GetParent<Control>();
		_target = null;
		_logger = new Logger(this);
	}

	private void SetTarget(Control node)
	{
		if (_target != null)
		{
			// Update focus
			_target = node;
			_targetHistory.AddLast(_target);
			if (_targetHistory.Count > 5)
			{
				_targetHistory.RemoveFirst();
			}
		}
		else
		{
			// First time focus
			_target = node;
			ForcePositionUpdate();
		}
	}

	public override void _Process(double delta)
	{
		if (_target is null) return;

		if (!IsInstanceValid(_target) || !_target.HasFocus())
		{
			_target = TryGetFallback();
			if (_target == null)
			{
				_logger.Debug("Focus lost. Stop tracking.");
				return;
			}
		}

		var position = CalcFocusedElementPosition(_target);
		position = AddOriginOffset(position);
		position = SmoothenPosition(position, delta);
		_self.GlobalPosition = position;
	}

	private Control TryGetFallback()
	{
		while (_targetHistory.Count > 0)
		{
			var control = _targetHistory.Last!.Value;
			_targetHistory.RemoveLast();
			if (!IsInstanceValid(control) || !control.IsVisibleInTree())
			{
				continue;
			}

			_logger.Debug("Fallback to last focused element." + control.Name);
			control.GrabFocus();
			return control;
		}

		return null;
	}

	private void ForcePositionUpdate()
	{
		var position = CalcFocusedElementPosition(_target);
		position = AddOriginOffset(position);
		_self.GlobalPosition = position;
	}

	private Vector2 CalcFocusedElementPosition(Control node)
	{
		var rect = node.GetGlobalRect();
		return PositionModeTarget switch
		{
			PositionMode.Origin => rect.Position,
			PositionMode.Center => rect.Position + rect.Size / 2,
			_ => Vector2.Zero
		};
	}

	private Vector2 AddOriginOffset(Vector2 position)
	{
		return PositionModeSelf switch
		{
			PositionMode.Origin => position,
			PositionMode.Center => position - _self.GetGlobalRect().Size / 2,
			_ => Vector2.Zero
		};
	}

	private Vector2 SmoothenPosition(Vector2 pos, double delta)
	{
		if (SmoothingFactor <= 0) return pos;
		return _self.GlobalPosition.Lerp(pos, SmoothingFactor * (float)delta);
	}
}
