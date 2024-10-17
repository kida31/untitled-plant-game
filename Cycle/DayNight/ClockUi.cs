using Godot;
using System;
using untitledplantgame.Common;

namespace untitledplantgame.Cycle.UI;


public partial class ClockUi : Control
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
		
		TimeController.Instance.TimeTick += SetDaytime;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetDaytime(int day, int hour, int minute)
	{
		_dayLabel.Text = "Day " + Convert.ToString(day + 1);
		_dayLabelBackground.Text = _dayLabel.Text;

		_timeLabel.Text = AmFm(hour) + ":" + SingleOrDoubleDigitMinute(minute) + " " + AmPm(hour);
		_timeLabelBackground.Text = _timeLabel.Text;

		if (hour <= 12)
		{
			_arrow.RotationDegrees = RemapRangeF( hour, 0, 12, -90, 90);
		}
		else
		{
			_arrow.RotationDegrees = RemapRangeF( hour, 13, 23, 90, -90);
		}

	}

	private static string AmFm(int hour)
	{
		if (hour == 0)
		{
			return Convert.ToString(12);
		}
		if (hour > 12)
		{
			return Convert.ToString(hour - 12);
		}

		return Convert.ToString(hour);
	}

	private string SingleOrDoubleDigitMinute(int minute)
	{
		if (minute < 10)
		{
			return "0" + Convert.ToString(minute);
		}
		return Convert.ToString(minute);
	}

	private string AmPm(int hour)
	{
		if (hour < 12) return "am";
		else return "pm";
	}

	private float RemapRangeF(double input, double minInput, double maxInput, double minOutput, double maxOutput)
	{
		return (float)((input - minInput) / (maxInput - minInput) * (maxOutput - minOutput) + minOutput);
	}
}
