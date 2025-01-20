using Godot;
using System;
using System.Collections.Generic;

namespace untitledplantgame.Common;

public partial class ClockBasedEventController : Node
{
	public static ClockBasedEventController Instance { get; private set; }
	
	private int _timeOfDayInMinutes;
	private event Action _clockBasedEventsInvoked;
	private readonly Dictionary<int, List<Action>> _scheduledEvents = new ();
	
	private Logger _logger;

	public override void _Ready()
	{
		if (Instance != null)
		{
			_logger.Warn("Multiple instances of TimeController found, deleting the new one");
			QueueFree();
			return;
		}
		
		Instance = this;
		
		_clockBasedEventsInvoked += InvokeClockBasedEvents;
	}

	public void SetTimeOfDayInMinutes(int timeOfDayInMinutes)
	{
		_timeOfDayInMinutes = timeOfDayInMinutes;
	}
	
	public void AddClockBasedEvent(int hour, int minute, Action action)
	{
		var eventTime = hour * 60 + minute;
		
		if (!_scheduledEvents.ContainsKey(eventTime))
		{
			_scheduledEvents[eventTime] = new List<Action>();
		}

		_scheduledEvents[eventTime].Add(action);
	}

	public void ExecuteAllClockBasedEvents()
	{
		_clockBasedEventsInvoked?.Invoke();
	}
	
	private void InvokeClockBasedEvents()
	{
		if (_scheduledEvents.TryGetValue(_timeOfDayInMinutes, out var @event))
		{
			foreach (var action in @event)
			{
				action?.Invoke();
			}
		}
	}
}
