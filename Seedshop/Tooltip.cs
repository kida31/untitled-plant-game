using System;
using Godot;

public partial class Tooltip : Popup
{
	public string origin = "";
	public string slot = "";
	public bool valid = false;

	public override void _Ready()
	{
		if (origin == "Seedshop" && slot != "")
		{
			GetNode<Label>("ColorRect/MarginContainer/VBoxContainer/Label").Text = slot;
			GD.Print(GetNode<Label>("ColorRect/MarginContainer/VBoxContainer/Label").Text);
			valid = true;
		}
		if (valid)
		{
			return;
		}
	}

	public override void _Process(double delta) { }
}