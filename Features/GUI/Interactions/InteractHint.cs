using Godot;

namespace untitledplantgame.GUI.Interactions;

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
