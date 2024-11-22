using System;
using Godot;
using untitledplantgame.Inventory.GUI;

public partial class Testtooltipresize : Control
{
	[Export]
	private TextEdit _textEdit;

	[Export]
	private TooltipView _tooltip;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_textEdit.TextChanged += OnTextChanged;
	}

	private void OnTextChanged()
	{
		_tooltip.Title = _textEdit.Text;
	}
}
