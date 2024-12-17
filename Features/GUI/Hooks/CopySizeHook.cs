using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.GUI.Hooks;

/// <summary>
/// This hook makes parent Control adjust its own size according to target node.
/// </summary>
public partial class CopySizeHook : Node
{
	[Export] public float SmoothingFactor = 10.0f;

	[Export]
	public Control Target
	{
		get => _target;
		set => _target = value;
	}

	private Control _target;
	private Control _self;
	private Logger _logger;
	private Vector2 _originalCenterPosition;

	public override void _Ready()
	{
		_self = GetParent<Control>();
		_originalCenterPosition = _self.Position + _self.Size / 2;
		_target = null;
		_logger = new Logger(this);
	}

	public override void _Process(double delta)
	{
		if (_target == null) return;

		var size = _target.GetGlobalRect().Size;
		size = SmoothenSize(size, delta);

		_self.Size = size;
		_self.Position = CalcLocalPosForSizeChange(_self.Size);
	}

	private Vector2 SmoothenSize(Vector2 size, double delta)
	{
		if (SmoothingFactor <= 0) return size;
		var w = Math.Min(1.0, SmoothingFactor * delta);
		return _self.Size.Lerp(size, (float) w);
	}

	private Vector2 CalcLocalPosForSizeChange(Vector2 size)
	{
		return _originalCenterPosition - size / 2;
	}
}
