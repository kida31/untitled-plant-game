using Godot;

namespace untitledplantgame.GUI.Interactions;

/// <summary>
///		A hint to show the player how to interact with a nearby object.
/// </summary>
[Tool]
public partial class InteractHint : Control
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
