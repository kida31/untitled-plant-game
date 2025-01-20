using Godot;
using System;
using System.Collections.Generic;

namespace untitledplantgame.Common;

public partial class ClockBasedEventController : Node
{
	public static ClockBasedEventController Instance { get; private set; }
	
	private int _timeOfDayInMinutes;
	private event Action ClockBasedEventsInvoked;
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

		TimeController.Instance.MinuteTicked += SetTimeOfDayInMinutes;
		ClockBasedEventsInvoked += InvokeClockBasedEvents;
	}

	private void SetTimeOfDayInMinutes(int _, int hour, int minute)
	{
		_timeOfDayInMinutes = hour * 60 + minute;
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
		ClockBasedEventsInvoked?.Invoke();
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
