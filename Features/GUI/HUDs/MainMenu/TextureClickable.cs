using Godot;
using untitledplantgame.GUI.Components;

namespace untitledplantgame.GUI.HUDs.MainMenu;

public partial class TextureClickable : Clickable
{
	[Export] private TextureRect _icon;
	[Export] private Texture2D _normal;
	[Export] private Texture2D _pressed;
	[Export] private Texture2D _hover;

	public TextureClickable() : base()
	{
		Pressed += OnPressed;
		SecondaryPressed += OnSecondaryPressed;
		FocusEntered += OnFocusEntered;
		FocusExited += OnFocusExited;
		GuiInput += OnGuiInput;
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (@event is not InputEventMouseButton mouseEvent)
		{
			return;
		}

		if (!mouseEvent.Pressed)
		{
			// Release
			_icon.Texture = _normal;
		}
	}

	private void OnPressed()
	{
		if (_pressed != null)
		{
			_icon.Texture = _pressed;
		}
	}

	private void OnSecondaryPressed()
	{
		if (_pressed != null)
		{
			_icon.Texture = _normal;
		}
	}

	private void OnFocusEntered()
	{
		if (_hover != null)
		{
			_icon.Texture = _hover;
		}
	}

	private void OnFocusExited()
	{
		if (_normal != null)
		{
			_icon.Texture = _normal;
		}
	}
}
