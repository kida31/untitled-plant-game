using Godot;
using System;

namespace untitledplantgame.DayNightCycle.UI;


public partial class DayNightCycleUi : Control
{

	private Label _dayLabelBackground;
	private Label _dayLabel;
	private Label _timeLabelBackground;
	private Label _timeLabel;
	private TextureRect _arrow;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_dayLabelBackground = GetNode<Label>("CenterContainerDay/DayLabelBackground");
		_dayLabel = GetNode<Label>("CenterContainerDay/DayLabelBackground/DayLabel");
		_timeLabelBackground = GetNode<Label>("CenterContainerTime/TimeLabelBackground");
		_timeLabel = GetNode<Label>("CenterContainerTime/TimeLabelBackground/TimeLabel");
		_arrow = GetNode<TextureRect>("Arrow");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
