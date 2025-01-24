using System;
using Godot;

namespace untitledplantgame.GUI.Components;

public partial class ScrollLabel : ScrollContainer
{
	[Export] private RichTextLabel _label; // Label or RichTextLabel

	public virtual string Text
	{
		get => _label.Text;
		set => _label.Text = value;
	}
}
