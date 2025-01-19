using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.GUI.Hooks;

namespace untitledplantgame.GUI.Selector;

// TODO: Move modulate out of class
/// <summary>
///		This is the visual indicator that follows the player's current selection/focus on GUI (in the book in particular).
///		This is disabled for freeroam state only
/// </summary>
public partial class UIFocusIcon : Control
{
	[ExportCategory("Hooks")] [Export] private CopySizeHook _sizeHook;
	[Export] private FollowControlHook _followHook;
	[Export] private float _modulateLerpFactor = 15.0f;

	public Control FocusedControl => _focusedControl;

	private Control _focusedControl;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);

		GetViewport().GuiFocusChanged += OnGuiFocusChanged;
		GameStateMachine.Instance.StateChanged += OnStateChanged;
	}

	public override void _Process(double delta)
	{
		if (_focusedControl == null || !_focusedControl.HasFocus() ||!_focusedControl.IsVisibleInTree())
		{
			Modulate = new Color(Modulate) {A = 0f};
		}
		else
		{
			Modulate = Modulate.Lerp(new Color(Modulate) {A = 1f}, (float) delta * _modulateLerpFactor);
		}
	}

	private void OnGuiFocusChanged(Control node)
	{
		if (GameStateMachine.Instance.CurrentState == GameState.FreeRoam)
		{
			_logger.Debug("Currently in FreeRoam. Ignoring focus change.");
			return;
		}

		// Update hooks
		_sizeHook.Target = node;
		_followHook.Target = node;

		// Manual adjustments
		_focusedControl = node;
		Modulate = new Color(Modulate) {A = 0f};
		_logger.Info("Focus new element: " + node.Name);
	}

	private void OnStateChanged(GameState previous, GameState newState)
	{
		if (newState != GameState.FreeRoam)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}
}
