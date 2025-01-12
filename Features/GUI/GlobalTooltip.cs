using System;
using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI;

/// <summary>
///		 Tracks global GUI focus to show tooltips for any control that implements ITooltipable
/// </summary>
public partial class GlobalTooltip : TooltipView
{
	/// <summary>
	/// Speed for fade in. Higher is faster (shorter animation). 0 or negative is instant
	/// </summary>
	[Export] private float _fadeInSpeed;

	/// <summary>
	/// Speed for fade out. Higher is faster (shorter animation).  0 or negative is instant
	/// </summary>
	[Export] private float _fadeOutSpeed;

	/// <summary>
	/// Delay after focus before tooltip is shown.  0 or negative is instant
	/// </summary>
	[Export] private float _delay;

	/// <summary>
	/// Offset for tooltip from center of focused object
	/// </summary>
	[Export] private Vector2 _offset;
	
	private bool HasContent => !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Description);
	
	private Control _target;
	private Timer _delayTimer;
	
	public override void _Ready()
	{
		base._Ready();

		GetViewport().GuiFocusChanged += OnGuiFocusChanged;
		
		_delayTimer = new Timer();
		_delayTimer.OneShot = true;
		AddChild(_delayTimer);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		var targetIsValid = _target != null &&
		                    _target.HasFocus() &&
		                    _target.IsVisibleInTree();
		var noOtherGuiActive = CursorInventory.Instance?.Content == null;
		if (
			targetIsValid &&
			HasContent &&
			noOtherGuiActive
		)
		{
			// Set position
			var rect = _target.GetGlobalRect();
			var center = rect.Position + rect.Size / 2;
			GlobalPosition = center + _offset;

			var weight = _fadeInSpeed <= double.Epsilon ? 1 : Math.Clamp(delta * _fadeInSpeed, 0, 1);
			Modulate = Modulate.Lerp(new Color(Modulate) { A = 1 }, (float)weight);
		}
		else
		{
			var weight = _fadeOutSpeed <= double.Epsilon ? 1 : Math.Clamp(delta * _fadeOutSpeed, 0, 1);
			Modulate = Modulate.Lerp(new Color(Modulate) { A = 0 }, (float)weight);
		}
	}

	private void SetContent()
	{
		var tooltipTarget = _target as ITooltipable;
		Title = tooltipTarget?.Title ?? "";
		Description = tooltipTarget?.Description ?? "";
		CustomContent = tooltipTarget?.Content != null ? new() { tooltipTarget.Content } : new();
	}
	
	private async void OnGuiFocusChanged(Control node)
	{
		// Clear Content
		_target = null;

		if (node is ITooltipable ttTarget && ttTarget.TooltipEnabled)
		{
			_delayTimer.Start(Math.Max(double.Epsilon, _delay));
			await ToSignal(_delayTimer, Timer.SignalName.Timeout);
			_target = node;
			SetContent();
		}
	}
}
