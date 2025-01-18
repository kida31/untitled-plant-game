using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.Inputs.GameActions;

namespace untitledplantgame.GUI.HUDs;

[Tool]
public partial class ButtonIndicator : Control
{
	private const string ButtonAssetsRoot = "res://Assets/UI/Buttons/";
	private const string XboxButtonAssetsRoot = ButtonAssetsRoot + "XBOX/";
	private const string PlaystationButtonAssetsRoot = ButtonAssetsRoot + "Playstation/";

	private const GamepadType DefaultGamepad = GamepadType.Xbox;

	enum GamepadType
	{
		Xbox,
		Playstation,
	}

	/// <summary>
	/// Combines Joybutton and JoyAxis
	/// </summary>
	public enum SimpleButton
	{
		South_A_X,
		East_B_O,
		West_X_Square,
		North_Y_Triangle,
		LeftShoulder_LB_L1,
		RightShoulder_RB_R1,
		LeftTrigger_LT_L2,
		RightTrigger_RT_R2,
		Start,
		Select,
	}

	private const string PressedAnimationName = "Pressed";
	private const string ReleasedAnimationName = "Released";

	/// <summary>
	/// Gamepad type for displayed button. This is a read only property
	/// </summary>
	[Export]
	private GamepadType Gamepad
	{
		get => DefaultGamepad;
		set
		{
			/* ignored */
		}
	}

	[Export]
	public SimpleButton Button
	{
		get => _button;
		set
		{
			_button = value;
			OnButtonChanged();
		}
	}

	[Export] private AnimationPlayer _animationPlayer;

	public bool IsPressed
	{
		get => _textureButton.IsPressed();
		set => _textureButton.SetPressed(value);
	}

	[Export] private TextureButton _textureButton;

	private SimpleButton _button;
	private StringName _action;

	private Logger _logger;
	// private bool _isPressed = false;

	public override void _Ready()
	{
		_logger = new Logger(this);
		_action = ButtonAsAction(_button); // bind button
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsAction(_action))
		{
			IsPressed = !@event.IsActionReleased(_action);
		}
	}

	private void OnButtonChanged()
	{
		_action = ButtonAsAction(_button);

		// BUG: On godot start or start of main scene,
		// this is sometimes being called and throws errors since [Export]s are not set
		// Reproducing this is not consistent for some reason
		// If condition is added to prevent this
		if (IsInsideTree())
		{
			AssignButtonImage(_button, Gamepad);
		}
	}

	private StringName ButtonAsAction(SimpleButton button)
	{
		switch (button)
		{
			case SimpleButton.South_A_X:
				return Base.South;
			case SimpleButton.East_B_O:
				return Base.East;
			case SimpleButton.West_X_Square:
				return Base.West;
			case SimpleButton.North_Y_Triangle:
				return Base.North;

			case SimpleButton.Start:
				return Base.Start;
			case SimpleButton.Select:
				return Base.Select;

			case SimpleButton.LeftShoulder_LB_L1:
				return Base.BumperLeft;
			case SimpleButton.RightShoulder_RB_R1:
				return Base.BumperRight;

			case SimpleButton.LeftTrigger_LT_L2:
				return Base.TriggerLeft;
			case SimpleButton.RightTrigger_RT_R2:
				return Base.TriggerRight;
		}

		_logger.Error("No action found for button: " + button);
		return null;
	}

	private void AssignButtonImage(SimpleButton button, GamepadType gamepadType)
	{
		switch (gamepadType, button)
		{
			case (GamepadType.Xbox, SimpleButton.South_A_X):
				_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "A_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "A_pressed.png");
				return;
			case (GamepadType.Xbox, SimpleButton.East_B_O):
				_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "B_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "B_pressed.png");
				return;
			case (GamepadType.Xbox, SimpleButton.West_X_Square):
				_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "X_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "X_pressed.png");
				return;
			case (GamepadType.Xbox, SimpleButton.North_Y_Triangle):
				_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "Y_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "Y_pressed.png");
				return;
			case (GamepadType.Xbox, SimpleButton.LeftShoulder_LB_L1):
				_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "LB_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "LB_pressed.png");
				return;
			case (GamepadType.Xbox, SimpleButton.RightShoulder_RB_R1):
				_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "RB_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "RB_pressed.png");
				return;

			case (GamepadType.Xbox, SimpleButton.LeftTrigger_LT_L2):
			case (GamepadType.Xbox, SimpleButton.RightTrigger_RT_R2):
			case (GamepadType.Xbox, SimpleButton.Start):
			case (GamepadType.Xbox, SimpleButton.Select):
				break;

			case (GamepadType.Playstation, SimpleButton.South_A_X):
				_textureButton.TextureNormal = TryLoadOrLogError(PlaystationButtonAssetsRoot + "X_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(PlaystationButtonAssetsRoot + "X_pressed.png");
				return;
			case (GamepadType.Playstation, SimpleButton.East_B_O):
				_textureButton.TextureNormal = TryLoadOrLogError(PlaystationButtonAssetsRoot + "circle_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(PlaystationButtonAssetsRoot + "circle_pressed.png");
				return;
			case (GamepadType.Playstation, SimpleButton.West_X_Square):
				_textureButton.TextureNormal = TryLoadOrLogError(PlaystationButtonAssetsRoot + "square_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(PlaystationButtonAssetsRoot + "square_pressed.png");
				return;
			case (GamepadType.Playstation, SimpleButton.North_Y_Triangle):
				_textureButton.TextureNormal = TryLoadOrLogError(PlaystationButtonAssetsRoot + "triangle_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(PlaystationButtonAssetsRoot + "triangle_pressed.png");
				return;
			case (GamepadType.Playstation, SimpleButton.LeftShoulder_LB_L1):
				_textureButton.TextureNormal = TryLoadOrLogError(PlaystationButtonAssetsRoot + "L1_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(PlaystationButtonAssetsRoot + "L1_pressed.png");
				return;
			case (GamepadType.Playstation, SimpleButton.RightShoulder_RB_R1):
				_textureButton.TextureNormal = TryLoadOrLogError(PlaystationButtonAssetsRoot + "R1_default.png");
				_textureButton.TexturePressed = TryLoadOrLogError(PlaystationButtonAssetsRoot + "R1_pressed.png");
				return;

			case (GamepadType.Playstation, SimpleButton.LeftTrigger_LT_L2):
			case (GamepadType.Playstation, SimpleButton.RightTrigger_RT_R2):
			case (GamepadType.Playstation, SimpleButton.Start):
			case (GamepadType.Playstation, SimpleButton.Select):
				break;
		}

		_logger.Error($"Gamepad Button not defined {gamepadType}::{button}");
	}

	private Texture2D TryLoadOrLogError(string resourcePath)
	{
		var result = GD.Load<Texture2D>(resourcePath);
		if (result == null)
		{
			_logger.Error("Failed to load resource: " + resourcePath);
		}

		return result;
	}
}
