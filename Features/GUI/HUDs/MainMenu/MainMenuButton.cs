using Godot;

namespace untitledplantgame.GUI.HUDs.MainMenu;

public partial class MainMenuButton : Button
{
	[Export] private TextureRect _textureRectToChange;
	[Export] private Texture2D _defaultTexture2D;
	[Export] private Texture2D _pressedTexture2D;
	
	// TODO: Check if the released event can be handled differently
	public override void _GuiInput(InputEvent @event)
	{
		if (@event is not InputEventMouseButton mouseEvent)
		{
			return;
		}

		if (mouseEvent.Pressed)
		{
			_textureRectToChange.Texture = _pressedTexture2D;
		}
		else
		{
			_textureRectToChange.Texture = _defaultTexture2D;

		}
	}
}
