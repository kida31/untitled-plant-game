using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Common;

[Singleton]
public partial class TimeController : Node
{
	/// Constants for time calculations
	private const double SecondsPerDay = 24 * 60 * 60;

	private const double InGameToRealTimeMultiplier = 60.0 * 10; // 1 second = 1min
	private const double InGameToRealTimeFastForwardMultiplier = SecondsPerDay; // 24h in 1s
	private const double StartOfDaySeconds = 7 * 60 * 60;

	/// The hour with which the day starts
	/// Singleton instance that's accessible from anywhere
	public static TimeController Instance { get; private set; }

	public delegate void DayChangedHandler(int day);

	public event DayChangedHandler DayChanged;

	public delegate void MinuteTickedHandler(int day, int hour, int minute);

	public event MinuteTickedHandler MinuteTicked;


	/// <summary>
	/// Current time in seconds
	/// </summary>
	public double CurrentSeconds { get; private set; }

	private Logger _logger;
	private int _pastDay; // For reference for DayChanged event
	private int _pastMinute = -1; // For reference for MinuteTicked event
	private double _fastForwardDuration = -1; // Gotta go fast juice. -1 means not fast forwarding. Consumed while fast forwarding

	public override void _Ready()
	{
		if (Instance != null)
		{
			_logger.Warn("Multiple instances of TimeController found, deleting the new one");
			QueueFree();
			return;
		}

		Instance = this;
		CurrentSeconds = StartOfDaySeconds;
		_logger = new Logger(this);
		_logger.Debug($"Time initialized with {CurrentSeconds}");
	}

	/**
	 * Called every frame. 'delta' is the elapsed time since the previous frame.
	 * _time gets updated every other frame depending on in-game time
	 */
	public override void _Process(double delta)
	{
		var dt = delta * (_fastForwardDuration > 0 ? InGameToRealTimeFastForwardMultiplier : InGameToRealTimeMultiplier);
		CurrentSeconds += dt;
		_fastForwardDuration -= dt;

		RecalculateTimeEvents();

		if (CurrentSeconds >= SecondsPerDay)
		{
			CurrentSeconds = 0;
		}
	}

	/**
	 * TODO: calculates in-game time every time tick (not every frame, as calculated in _Process())
	 * emits a signal with the current time
	 */
	private void RecalculateTimeEvents()
	{
		const double minutesPerDay = 24 * 60;
		const double minutesPerHour = 60;

		var totalMinutes = (int) (CurrentSeconds / 60);

		var day = (int) (totalMinutes / minutesPerDay);
		var currentDayMinutes = (int) (totalMinutes % minutesPerDay);
		var hour = (int) (currentDayMinutes / minutesPerHour);
		var minute = (int) (currentDayMinutes % minutesPerHour);

		if (_pastDay != day)
		{
			_logger.Debug($"Day {day} passed, emitting signal");
			_pastDay = day;
			DayChanged?.Invoke(day);
		}

		if (_pastMinute != minute)
		{
			_pastMinute = minute;
			MinuteTicked?.Invoke(day, hour, minute);
		}
	}

	public void FastForwardToNextDay()
	{
		FastForwardTo(StartOfDaySeconds);
	}

	//Please remove this later
	public void FastForwardFor(double duration)
	{
		Assert.AssertTrue(duration > 0);
		_fastForwardDuration = duration;
	}

	public void FastForwardTo(double targetTime)
	{
		Assert.AssertTrue(targetTime < SecondsPerDay, "Target time is greater than a day");
		FastForwardFor((SecondsPerDay + targetTime - CurrentSeconds) % SecondsPerDay);
	}
}
