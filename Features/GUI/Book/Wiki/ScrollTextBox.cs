using Godot;
using System;

public partial class ScrollTextBox : ScrollContainer
{
	public event Action TextChanged;
	
	[Export] private Label _label;

	public string Text
	{
		get => _label.Text;
		set
		{
			_label.Text = value;
			TextChanged?.Invoke();
		}
	}
}
