using Godot;
using System;
using untitledplantgame.Common.GameStates;

/// <summary>
/// A book button.
/// Has two visual states: open and closed.
///
/// Per default the book reacts to mouse hover and GameState.Book.
/// Extract this behaviour if it needs to be reused.
/// </summary>
public partial class BookButton : Control
{
	[Export] private AnimationPlayer _animationPlayer;
	public event Action Pressed;

	public bool IsOpen
	{
		get => _isOpen;
		set => SetOpen(value);
	}

	private bool _isOpen;
	private Timer _tooLazyForConsistentUpdateLogicTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Update content every 5 seconds
		_tooLazyForConsistentUpdateLogicTimer = new Timer();
		_tooLazyForConsistentUpdateLogicTimer.WaitTime = 2.0f;
		_tooLazyForConsistentUpdateLogicTimer.Autostart = true;
		_tooLazyForConsistentUpdateLogicTimer.OneShot = false;
		_tooLazyForConsistentUpdateLogicTimer.Timeout += UpdateViewContent;
		AddChild(_tooLazyForConsistentUpdateLogicTimer);

		// This is arbitrary behaviour. To have a more modular component extract the following behaviour
		MouseEntered += UpdateViewContent;
		MouseExited += UpdateViewContent;
		GameStateMachine.Instance.StateChanged += (_, _) => UpdateViewContent();
		UpdateViewContent();
		// End of arbitrary behaviour
	}

	public override void _GuiInput(InputEvent @event)
	{
		var getIsClicked = () => @event is InputEventMouseButton {Pressed: true, ButtonIndex: MouseButton.Left};

		if (getIsClicked() || @event.IsActionPressed("ui_accept"))
		{
			Pressed?.Invoke();
		}
	}

	/// <summary>
	/// Update the view content according to mouse hover and/or game state.
	/// This behaviour is arbitrary and can be extracted. (Set by parent instead)
	/// </summary>
	private void UpdateViewContent()
	{
		var hasMouseHover = GetGlobalRect().HasPoint(GetGlobalMousePosition());
		// Hover as a soft way to "open" the book.
		// GameState.Book overrides this.
		SetOpen(hasMouseHover || GameStateMachine.Instance.CurrentState == GameState.Book);
	}

	/// <summary>
	/// Sets isOpen. If new state is the same as the current state, do nothing.
	/// If new state is different, animate the change.
	/// </summary>
	/// <param name="newIsOpen"></param>
	private void SetOpen(bool newIsOpen)
	{
		if (newIsOpen == _isOpen)
		{
			// Do nothing if same
			return;
		}

		_isOpen = newIsOpen;

		// Animate new open state
		if (_isOpen)
		{
			_animationPlayer.PlayBackwards("FadeClose");
		}
		else
		{
			_animationPlayer.Play("FadeClose");
		}
	}
}
