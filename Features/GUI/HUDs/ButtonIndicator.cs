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
	public JoyButton Button
	{
		get => _button;
		set
		{
			_button = value;
			OnButtonChanged();
		}
	}

	[Export] private TextureButton _textureButton;
	[Export] private AnimationPlayer _animationPlayer;

	public bool IsPressed
	{
		get => _textureButton.IsPressed();
		set => _textureButton.SetPressed(value);
	}

	private JoyButton _button;
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
			IsPressed = !@event.IsReleased();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventJoypadButton button)
		{
			GD.Print(button.AsText());
		}
	}

	private void OnButtonChanged()
	{
		_action = ButtonAsAction(_button);
		AssignButtonImage(_button, Gamepad);
	}

	private StringName ButtonAsAction(JoyButton button)
	{
		switch (button)
		{
			case JoyButton.A:
				return Base.South;
			case JoyButton.B:
				return Base.East;
			case JoyButton.X:
				return Base.West;
			case JoyButton.Y:
				return Base.North;

			case JoyButton.Start:
				return Base.Start;

			case JoyButton.LeftShoulder:
				return Base.BumperLeft;
			case JoyButton.RightShoulder:
				return Base.BumperRight;

			case JoyButton.DpadUp:
				return Base.Up;
			case JoyButton.DpadDown:
				return Base.Down;
			case JoyButton.DpadLeft:
				return Base.Left;
			case JoyButton.DpadRight:
				return Base.Right;
			default:
				_logger.Error("No action found for button: " + button);
				return null;
		}
	}

	private void AssignButtonImage(JoyButton button, GamepadType gamepadType)
	{
		switch (gamepadType)
		{
			case GamepadType.Xbox:
				switch (button)
				{
					case JoyButton.A:
						_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "A_default.png");
						_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "A_pressed.png");
						return;
					case JoyButton.B:
						_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "B_default.png");
						_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "B_pressed.png");
						return;
					case JoyButton.X:
						_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "X_default.png");
						_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "X_pressed.png");
						return;
					case JoyButton.Y:
						_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "Y_default.png");
						_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "Y_pressed.png");
						return;
					case JoyButton.LeftShoulder:
						_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "LB_default.png");
						_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "LB_pressed.png");
						return;
					case JoyButton.RightShoulder:
						_textureButton.TextureNormal = TryLoadOrLogError(XboxButtonAssetsRoot + "RB_default.png");
						_textureButton.TexturePressed = TryLoadOrLogError(XboxButtonAssetsRoot + "RB_pressed.png");
						return;
					case JoyButton.Invalid:
					case JoyButton.Back:
					case JoyButton.Guide:
					case JoyButton.Start:
					case JoyButton.LeftStick:
					case JoyButton.RightStick:
					case JoyButton.DpadUp:
					case JoyButton.DpadDown:
					case JoyButton.DpadLeft:
					case JoyButton.DpadRight:
					case JoyButton.Misc1:
					case JoyButton.Paddle1:
					case JoyButton.Paddle2:
					case JoyButton.Paddle3:
					case JoyButton.Paddle4:
					case JoyButton.Touchpad:
					case JoyButton.SdlMax:
					case JoyButton.Max:
					default:
						_logger.Error("Button not found: " + button);
						return;
				}
			case GamepadType.Playstation:
				switch (button)
				{
					case JoyButton.A:
						return;
					case JoyButton.Invalid:
					case JoyButton.B:
					case JoyButton.X:
					case JoyButton.Y:
					case JoyButton.Back:
					case JoyButton.Guide:
					case JoyButton.Start:
					case JoyButton.LeftStick:
					case JoyButton.RightStick:
					case JoyButton.LeftShoulder:
					case JoyButton.RightShoulder:
					case JoyButton.DpadUp:
					case JoyButton.DpadDown:
					case JoyButton.DpadLeft:
					case JoyButton.DpadRight:
					case JoyButton.Misc1:
					case JoyButton.Paddle1:
					case JoyButton.Paddle2:
					case JoyButton.Paddle3:
					case JoyButton.Paddle4:
					case JoyButton.Touchpad:
					case JoyButton.SdlMax:
					case JoyButton.Max:
					default:
						_logger.Error("Button not found: " + button);
						return;
				}
			default:
				_logger.Error($"Gamepad Button not found {gamepadType}::{button}");
				return;
		}
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
