using Godot;
using System;
using untitledplantgame.GUI.Interactions;

/// <summary>
///		Small horizontal notification. Shrinks and grows vertically while masking child/content.
///		This may need refactoring to be more generic.
/// </summary>
public partial class MiniNotification : GenericTextControl
{
	[Export] private Control _content;
	[Export] private TextureRect _icon;

	public Control Content => _content;

	public float VScale
	{
		get => (Size.Y / _content?.Size.Y) ?? 0f;
		set
		{
			var size = _content.Size;
			size.Y *= value;
			CustomMinimumSize = size;
			Size = size;
		}
	}

	public Texture2D Texture
	{
		get => _icon?.Texture;
		set => _icon!.Texture = value;
	}
}
