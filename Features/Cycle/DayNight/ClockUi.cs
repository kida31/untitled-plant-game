using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Cycle.DayNight;

public partial class ClockUi : Control
{
	private Label _dayLabel;
	private Label _timeLabel;
	private TextureRect _arrow;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_dayLabel = GetNode<Label>("%DayLabel");
		_timeLabel = GetNode<Label>("%TimeLabel");
		_arrow = GetNode<TextureRect>("%Arrow");

		TimeController.Instance.TimeTick += SetDaytime;
	}

	private void SetDaytime(int day, int hour, int minute)
	{
		_dayLabel.Text = "Day " + Convert.ToString(day + 1);

		_timeLabel.Text = AmFm(hour) + ":" + SingleOrDoubleDigitMinute(minute) + " " + AmPm(hour);

		if (hour <= 12)
		{
			_arrow.RotationDegrees = RemapRangeF(hour + minute / 60f, 0, 13, -90, 90);
		}
		else
		{
			_arrow.RotationDegrees = RemapRangeF(hour + minute / 60f, 13, 24, 90, -90);
		}
	}

	private static string AmFm(int hour)
	{
		return hour switch
		{
			0 => Convert.ToString(12),
			> 12 => Convert.ToString(hour - 12),
			_ => Convert.ToString(hour),
		};
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
		if (hour < 12)
			return "am";
		else
			return "pm";
	}

	private float RemapRangeF(double input, double minInput, double maxInput, double minOutput, double maxOutput)
	{
		return (float)((input - minInput) / (maxInput - minInput) * (maxOutput - minOutput) + minOutput);
	}
}
