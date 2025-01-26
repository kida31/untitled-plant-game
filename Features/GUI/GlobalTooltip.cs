using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI;

/// <summary>
///		 Tracks global GUI focus to show tooltips for any control that implements ITooltipable
/// </summary>
public partial class GlobalTooltip : TooltipView
{
	/// <summary>
	/// Speed for fade in. Higher is faster (shorter animation). 0 is instant
	/// </summary>
	[Export] private float _fadeInSpeed;

	/// <summary>
	/// Delay after focus before tooltip is shown. Higher is slower. 0 is instant
	/// </summary>
	[Export] private float _delay;

	/// <summary>
	/// Offset for tooltip from center of focused object
	/// </summary>
	[Export] private Vector2 _offset;


	private Control _target;
	private Tween _fadeTween;
	private Timer _delayTimer;
	private Logger _logger;
	private bool HasContent => !string.IsNullOrEmpty(Title) || !string.IsNullOrEmpty(Description) || CustomContent != null;

	public override void _Ready()
	{
		base._Ready();

		GetViewport().GuiFocusChanged += OnGuiFocusChanged;

		_delayTimer = new Timer();
		_delayTimer.OneShot = true;
		_delayTimer.Timeout += SetContent;
		AddChild(_delayTimer);

		_logger = new Logger(this);
	}

	public override void _Process(double delta)
	{
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

			// Only allow tooltip to be positioned within viewport
			var vpRect = GetViewport().GetVisibleRect();
			var validArea = Vector2.Zero.Max(vpRect.Position + vpRect.Size - GetGlobalRect().Size);
			GlobalPosition = GlobalPosition.Clamp(Vector2.Zero, validArea);

			var weight = _fadeInSpeed > 0 ? delta * _fadeInSpeed : 1;
			Modulate = Modulate.Lerp(new Color(Modulate) {A = 1}, (float) weight);
			Show();
		}
		else
		{
			Hide();
		}
	}

	private void SetContent()
	{
		var tooltipable = _target as ITooltipable;

		Title = tooltipable?.Title ?? "";
		Description = tooltipable?.Description ?? "";

		CustomContent = tooltipable?.Content;
	}

	private void OnGuiFocusChanged(Control node)
	{
		// Clear Content
		_target = null;
		SetContent();

		// Set Content delayed
		Modulate = new Color(Modulate) {A = 0};
		_target = node is ITooltipable ? node : null;
		var delay = _delay > 0 ? _delay : double.Epsilon;
		_delayTimer.Start(delay);
	}
}
