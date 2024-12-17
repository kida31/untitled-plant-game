using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;

namespace untitledplantgame.Inventory.GUI;

public partial class FocusIndicator : Control
{
	/// <summary>
	/// Anchor defines the center position of the indicator. The control itself will have size of zero.
	/// </summary>
	[Export] private Control _anchor;
	/// <summary>
	/// Indicator object. Will be scaled to match the size of the focused control.
	/// </summary>
	[Export] private Control _indicator;

	private Tween _tween;
	private Control _focusedControl;
	private Logger _logger;

	[Export] private float _initialBounceEffect = .1f;
	[Export] private float _lerpSpeed = 15.0f;
	[Export] private float _tweenDuration = 0.2f;

	public override void _Ready()
	{
		_logger = new Logger(this);
		GetViewport().GuiFocusChanged += OnGuiFocusChanged;
		GameStateMachine.Instance.StateChanged += OnStateChanged;
		
		_anchor.TopLevel = true;
		_anchor.SetHGrowDirection(GrowDirection.Both);
		_anchor.SetVGrowDirection(GrowDirection.Both);
	}

	public override void _Process(double delta)
	{
		if (_focusedControl == null)
		{
			return;
		}

		LerpAnimation(_focusedControl, delta);
	}

	private void OnStateChanged(GameState previous, GameState newState)
	{
		if (newState == GameState.Book)
		{
			Show();
		}
		else
		{
			Hide();
			_focusedControl = null;
		}
	}

	private void OnGuiFocusChanged(Control node)
	{
		if (GameStateMachine.Instance.CurrentState != GameState.Book)
		{
			return;
		}

		var wasFocused = _focusedControl != null;
		_focusedControl = node;
		_indicator.Modulate = new Color(_indicator.Modulate) {A = 0f}; // Initially faded out
		if (!wasFocused)
		{
			ForceMove(node);
		}
	}

	private void LerpAnimation(Control node, double delta)
	{
		var rect = node.GetGlobalRect();
		
		var rectCenter = rect.Position + rect.Size / 2;
		// Pos
		_anchor.GlobalPosition = _anchor.GlobalPosition.Lerp(rectCenter, (float) delta * _lerpSpeed);
		// Size
		_indicator.Size = _indicator.Size.Lerp(rect.Size, (float) delta * _lerpSpeed);
		_indicator.Position = - _indicator.Size / 2;
		// Opacity
		_indicator.Modulate = _indicator.Modulate.Lerp(new Color(Modulate) {A = 1f}, (float) delta * _lerpSpeed);
	}

	private void ForceMove(Control node)
	{
		var rect = node.GetGlobalRect();
		var rectCenter = rect.Position + rect.Size / 2;
		_anchor.GlobalPosition = rectCenter;
	}

	private void TweenAnimation(Control node)
	{
		throw new NotImplementedException();
		_tween?.Stop();

		var rect = node.GetGlobalRect();
		// _logger.Debug($"Position={_indicatorRect.GlobalPosition} Size={_indicatorRect.Size}");
		_tween = CreateTween();
		_tween.TweenProperty(_anchor, PropertyName.Size.ToString(), rect.Size, 0.2f);
		_tween.Parallel().TweenProperty(_anchor, PropertyName.GlobalPosition.ToString(), rect.Position, 0.2f);
	}
}
