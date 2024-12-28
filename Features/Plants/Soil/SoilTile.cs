using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Cycle.Weather;

namespace untitledplantgame.Plants.Soil;

public partial class SoilTile : Area2D, IWaterable
{
	[Export] public float Hydration { get; private set; }
	public event Action<float> HydrationChanged;
	private float _maxHydration = 250;
	private float Fertilization { get; set; }
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		WeatherCycle.Instance.WeatherChanged += OnWeatherChanged;
		TimeController.Instance.DayChanged += OnDayChanged;
	}

	private void OnDayChanged(int day)
	{
		var weather = WeatherCycle.Instance.CurrentWeather;
		OnWeatherChanged(weather);
	}

	private void OnWeatherChanged(Weather obj)
	{
		switch (obj)
		{
			case Weather.Sunny:
				WithdrawHydration(30);
				break;
			case Weather.Rainy:
				AddWater(100);
				break;
			case Weather.Snowy:
				AddWater(50);
				break;
			case Weather.Cloudy:
				WithdrawHydration(20);
				break;
			default:
				_logger.Warn("Soil is confused about the weather.");
				return;
		}
	}

	public float WithdrawHydration(float reductionValue)
	{
		var prevHydration = Hydration;
		Hydration = Math.Clamp(Hydration - reductionValue, 0, Hydration);

		HydrationChanged?.Invoke(Hydration);

		return prevHydration - Hydration;
	}

	public void AddWater(float addedWater)
	{
		Hydration = Math.Min(Hydration + addedWater, _maxHydration);
		HydrationChanged?.Invoke(Hydration);
	}
}
