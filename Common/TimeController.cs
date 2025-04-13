using System;
using Godot;

namespace untitledplantgame.Common;

[Singleton]
public partial class TimeController : Node
{
	/// Constants for time calculations
	private const double SecondsPerDay = 24 * 60 * 60;

	private const double InGameToRealTimeMultiplier = 60.0; // 1 second = 1min
	private const double InGameToRealTimeFastForwardMultiplier = SecondsPerDay; // 24h in 1s
	private const double StartOfDaySeconds = 7 * 60 * 60;

	/// The hour with which the day starts
	/// Singleton instance that's accessible from anywhere
	public static TimeController Instance { get; private set; }

	public delegate void DayChangedHandler(int day);

	/// <summary>
	///		Invoked when the day changes. Passes the new day as an argument.
	/// </summary>
	public event DayChangedHandler DayChanged;

	/// <summary>
	///		 Invoked when the time is noon.
	/// </summary>
	public event Action NoonOccured;

	public delegate void MinuteTickedHandler(int day, int hour, int minute);

	/// <summary>
	///		 Invoked every minute. Passes the day, hour, and minute as arguments.
	/// </summary>
	public event MinuteTickedHandler MinuteTicked;


	/// <summary>
	///		Current time in seconds
	/// </summary>
	public double CurrentSeconds { get; private set; }

	private Logger _logger;
	private int _currentDay; // For reference for DayChanged event
	private int _currentMinute = -1; // For reference for MinuteTicked event
	private double _fastForwardDuration = -1; // Gotta go fast juice. -1 means not fast forwarding. Consumed while fast forwarding
	private double _currentTimeMultiplier = InGameToRealTimeMultiplier; // Multiplier for time speed
	private bool _wasNoon;
	private bool _isRunning = true;

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
		_wasNoon = false;
		_logger = new Logger(this);
		_logger.Debug($"Time initialized with {CurrentSeconds}");
	}
	
	public override void _Process(double delta)
	{
		if (!_isRunning) return;
		
		double dt;
		if (_fastForwardDuration < 0)
		{
			dt = delta * InGameToRealTimeMultiplier;
			_currentTimeMultiplier = InGameToRealTimeMultiplier;
		}
		else
		{
			_currentTimeMultiplier = Mathf.Lerp(_currentTimeMultiplier, InGameToRealTimeFastForwardMultiplier, delta * 0.1);
			dt = delta * _currentTimeMultiplier;
		}

		CurrentSeconds += dt;
		_fastForwardDuration -= dt;

		RecalculateTimeEvents();
	}
	
	/// <summary>
	///		Skips to the next day.
	/// </summary>
	public void GoToNextDay()
	{
		FastForwardTo(StartOfDaySeconds);
		_currentTimeMultiplier = InGameToRealTimeFastForwardMultiplier;
	}

	/// <summary>
	///		 Fast forwards the time by the given duration.
	/// </summary>
	/// <param name="duration"></param>
	public void FastForwardFor(double duration)
	{
		Assert.AssertTrue(duration > 0);
		_fastForwardDuration = duration;
	}

	/// <summary>
	///		 Fast forwards the time to the given target time.
	/// </summary>
	/// <param name="targetTime"></param>
	public void FastForwardTo(double targetTime)
	{
		Assert.AssertTrue(targetTime < SecondsPerDay, "Target time is greater than a day");
		FastForwardFor((SecondsPerDay + targetTime - CurrentSeconds) % SecondsPerDay);
	}
	
	public void Pause()
	{
		_logger.Debug("Pausing time");
		_isRunning = false;
	}
	
	public void Resume()
	{
		_logger.Debug("Resuming time");
		_isRunning = true;
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

		var currentDayMinutes = (int) (totalMinutes % minutesPerDay);
		var hour = (int) (currentDayMinutes / minutesPerHour);
		var minute = (int) (currentDayMinutes % minutesPerHour);

		if (CurrentSeconds >= SecondsPerDay)
		{
			_logger.Debug($"Day {_currentDay} passed, emitting signal");
			DayChanged?.Invoke(_currentDay);
			_currentDay += 1;
			CurrentSeconds = 0;
			_wasNoon = false;
		}

		if (_currentMinute != minute)
		{
			_currentMinute = minute;
			MinuteTicked?.Invoke(_currentDay, hour, minute);
		}

		if (currentDayMinutes >= 12 * 60 && !_wasNoon)
		{
			_wasNoon = true;
			NoonOccured?.Invoke();
		}
	}
}
