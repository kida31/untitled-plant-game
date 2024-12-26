using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.GUI.Hooks;

namespace untitledplantgame.GUI;

// TODO: Move modulate out of class
/// <summary>
/// This is the visual indicator that follows the player's current selection/focus on GUI (in the book in particular)
/// </summary>
public partial class UIFocusIcon : Control
{
	[ExportCategory("Hooks")] [Export] private CopySizeHook _sizeHook;
	[Export] private FollowControlHook _followHook;

	private Control _focusedControl;
	private Logger _logger;

	[Export] private float _modulateLerpFactor = 15.0f;

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
		if (GameStateMachine.Instance.CurrentState != GameState.Book)
		{
			return;
		}

		// Update hooks
		_sizeHook.Target = node;
		_followHook.Target = node;

		// Manual adjustments
		_focusedControl = node;
		Modulate = new Color(Modulate) {A = 0f};
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
		}
	}
}
