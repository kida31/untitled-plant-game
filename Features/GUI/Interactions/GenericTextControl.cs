using Godot;

namespace untitledplantgame.GUI.Interactions;

/// <summary>
///		A simple wrapper to access a remote RichTextLabel.
///		May be used to create generic control objects with nested text controls.
/// </summary>
[Tool]
public partial class GenericTextControl : Control
{
	[Export]
	public string Text
	{
		get => _label?.Text;
		set
		{
			if (_label != null) _label.Text = value;
		}
	}

	[Export] private RichTextLabel _label;
}
