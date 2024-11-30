using Godot;
using System;

public partial class BookButton : Control
{
	[Export] private TextureRect _bookIconOpen;
	[Export] private TextureRect _bookIconClosed;
	[Export] private Control _inputIndicator;
	// [Export] private Texture2D _iconOpen;
	// [Export] private Texture2D _iconClosed;
	
	public event Action Pressed;

	private Timer _tooLazyForUpdateLogicTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Update content every 5 seconds
		_tooLazyForUpdateLogicTimer = new Timer();
		_tooLazyForUpdateLogicTimer.WaitTime = 2.0f;
		_tooLazyForUpdateLogicTimer.Autostart = true;
		_tooLazyForUpdateLogicTimer.OneShot = false;
		_tooLazyForUpdateLogicTimer.Timeout += UpdateView;
		AddChild(_tooLazyForUpdateLogicTimer);
		
		MouseEntered += UpdateView;
		MouseExited += UpdateView;
		
		UpdateView();
	}

	public override void _GuiInput(InputEvent @event)
	{
		var getIsClicked = () => @event is InputEventMouseButton {Pressed: true, ButtonIndex: MouseButton.Left};

		if (getIsClicked() || @event.IsActionPressed("ui_accept"))
		{
			GD.Print("Clicked");
			Pressed?.Invoke();
		}
	}
	
	private void UpdateView()
	{
		var hasMouseHover = GetGlobalRect().HasPoint(GetGlobalMousePosition());
		GD.Print(GetGlobalMousePosition());
		// GD.Print($"{GetGlobalRect()}: {GetGlobalMousePosition()}: {hasMouseHover}");

		_bookIconClosed.Visible = !hasMouseHover;
		_bookIconOpen.Visible = hasMouseHover;
		var inputIndicatorModulate = _inputIndicator.Modulate;
		inputIndicatorModulate.A = hasMouseHover ? 0.5f : 1.0f;
		_inputIndicator.Modulate = inputIndicatorModulate;
	}
}
