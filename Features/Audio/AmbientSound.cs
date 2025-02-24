using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Cycle.Weather;

namespace untitledplantgame.Audio;

public partial class AmbientSound : AudioStreamPlayer
{
	private const string OceanTrackName = "Ocean";
	private const string RainTrackName = "Rain";
	private const string StormTrackName = "RainThunder";
	private const string WindTrackName = "Wind";
	private const string BirdsTrackName = "WindBirds";

	public static AmbientSound Instance { get; private set; }

	private Logger _logger;
	private Location? _currentLocation;
	private StringName _currentTrack;

	public override void _Ready()
	{
		if (Instance != null)
		{
			_logger.Error("Multiple instances of AmbientSound detected. Deleting this instance.");
			QueueFree();
			return;
		}

		Instance = this;
		_logger = new(this);

		EventBus.Instance.BgmAreaChanged += OnBgmAreaChanged;
		TimeController.Instance.MinuteTicked += OnTimeUpdated;
		Play();
	}

	private void OnTimeUpdated(int day, int hour, int minute)
	{
		if (!_currentLocation.HasValue) return;
		SetAmbient(WeatherCycle.Instance.CurrentWeather, _currentLocation.Value, hour);
	}

	private void OnBgmAreaChanged(IBgmArea area)
	{
		if (area is BgmArea2D areaWithLoc)
		{
			_currentLocation = areaWithLoc.GetLocation();
		}
	}

	private void SetAmbient(Weather weather, Location location, int hour)
	{
		if (!IsNodeReady()) return;

		// Muted music when in-doors
		var db = Mathf.LinearToDb(location == Location.House ? 0.3f : 1.0f);
		if (Math.Abs(VolumeDb - db) > double.Epsilon)
		{
			var tween = CreateTween();
			tween.TweenProperty(this, "volume_db", db, 0.2f);
		}

		if (weather == Weather.Rainy)
		{
			// Rain >> any other rule
			PlayIfNotAlreadyPlaying(RainTrackName);
			return;
		}

		if (hour > 20 || hour < 6)
		{
			// Late night == only winds
			PlayIfNotAlreadyPlaying(WindTrackName);
			return;
		}

		switch (location)
		{
			case Location.Garden:
			case Location.House:
				PlayIfNotAlreadyPlaying(BirdsTrackName);
				break;
			case Location.Pier:
				PlayIfNotAlreadyPlaying(OceanTrackName);
				break;
		}
	}

	private void PlayIfNotAlreadyPlaying(StringName track)
	{
		if (_currentTrack == track) return;
		_currentTrack = track;
		var pb = (AudioStreamPlaybackInteractive) GetStreamPlayback();
		_logger.Info($"Switching to {track}");
		pb.SwitchToClipByName(track);
	}
}
