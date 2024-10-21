using Godot;
using System;
using untitledplantgame.Common;

namespace untitledplantgame.Common;

[Singleton]
public partial class TimeController : Node
{
	/// Singleton instance that's accessible from anywhere
	public static TimeController Instance { get; private set; }
	/// Radial time in radians
	public double Time { get; private set; }

	private readonly Logger _logger = new ("Time");

	/// Constants for time calculations
	private const double MinutesPerDay = 1440;
	private const double MinutesPerHour = 60;
	private const double InGameToRealMinuteDuration = (2 * Math.PI) / MinutesPerDay;
	/// The speed at which in-game time passes
	private const double TickSpeed = 20.0;
	/// The hour with which the day starts
	private const int InitialHour = 7;

	private int _pastDay;
	private int _pastMinute = -1;

	[Signal]
	public delegate void DayChangedEventHandler(int day);

	[Signal]
	public delegate void TimeTickEventHandler(int day, int hour, int minute);

	public override void _Ready()
	{
		if (Instance != null)
		{
			_logger.Warn("Multiple instances of TimeController found, deleting the new one");
			QueueFree();
			return;
		}

		Instance = this;
		Time = InGameToRealMinuteDuration * InitialHour * MinutesPerHour;
		_logger.Debug($"Time initialized with {Time}");
	}

	/**
	 * Called every frame. 'delta' is the elapsed time since the previous frame.
	 * _time gets updated every other frame depending on in-game time
	 */
	public override void _Process(double delta)
	{
		Time += delta * InGameToRealMinuteDuration * TickSpeed;
		RecalculateTime();
	}

	/**
	 * calculates in-game time every time tick (not every frame, as calculated in _Process())
	 * emits a signal with the current time
	 */
	private void RecalculateTime()
	{
		var totalMinutes = (int)(Time / InGameToRealMinuteDuration);

		var day = (int)(totalMinutes / MinutesPerDay);
		var currentDayMinutes = (int)(totalMinutes % MinutesPerDay);
		var hour = (int)(currentDayMinutes / MinutesPerHour);
		var minute = (int)(currentDayMinutes % MinutesPerHour);

		if (_pastDay != day)
		{
			_logger.Debug($"Day {day} passed, emitting signal");
			_pastDay = day;
			EmitSignal(SignalName.DayChanged, day);
		}

		if (_pastMinute != minute)
		{
			_pastMinute = minute;
			EmitSignal(SignalName.TimeTick, day, hour, minute);
		}
	}
	
	//Please remove this later
	public void FastForwardFor(double targetTime)
	{
		Time += targetTime;
		RecalculateTime();
	}
}
