using Godot;
using System;
using untitledplantgame.Common.Inputs;
using untitledplantgame.Common.Inputs.GameActions;

public partial class ButtonIndicator : Control
{
	enum Button
	{
		
	}
	
	private const string PressedAnimationName = "Pressed";
	private const string ReleasedAnimationName = "Released";
	
	[Export] private Label _textLabel;
	[Export] private AnimationPlayer _animationPlayer;
	[Export] public string PrefabBoundAction = FreeRoam.TriggerLeft;

	public override void _Ready()
	{
		BindAction(PrefabBoundAction);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsAction(_boundAction))
		{
			IsPressed = !@event.IsReleased();
		}
	}

	public bool IsPressed
	{
		get => _isPressed;
		set
		{
			if (_isPressed == value)
			{
				return;
			}
			
			_isPressed = value;
			_animationPlayer.Play(_isPressed ? PressedAnimationName : ReleasedAnimationName);
		}
	}
	
	public void BindAction(string action)
	{
		_boundAction = action;
		if (_boundAction != null)
		{

		}
	}
	
	private string _boundAction;
	private bool _isPressed = false;
	
	// Helper
	private static Texture2D ButtonAsImage(JoyButton button)
	{
		return null;
	}
}
