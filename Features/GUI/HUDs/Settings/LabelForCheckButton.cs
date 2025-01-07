using Godot;

namespace untitledplantgame.GUI.HUDs.Settings;

public partial class LabelForCheckButton : Label
{
	[Export] private CheckButton _checkButton;
	
	private string _on;
	private string _off;

	public override void _Ready()
	{
		_checkButton.Pressed += ChangeText;
	}
	
	
	private void ChangeText()
	{
		// Set DEBUG MODE in here.
		Text = _checkButton.IsPressed() ? _on : _off;
	}
	
	public void SetDescriptiveLabelForCheckButton(string textForOn, string textForOff)
	{
		_on = textForOn;
		_off = textForOff;
	}
}
