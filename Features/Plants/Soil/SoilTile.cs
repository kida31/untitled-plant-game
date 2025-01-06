using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Cycle.Weather;

namespace untitledplantgame.Plants;

public partial class SoilTile : Area2D, IWaterable
{
	private const float MaxHydration = 300;
	private const float SunnyEvaporationRate = 50;
	private const float CloudyEvaporationRate = 50;
	private const float RainyHydrationRate = 100;
	private const float SnowyHydrationRate = 50;
	
	[Export] public float Hydration { get; private set; }
	public event Action<float, SoilTile> HydrationChanged;
	private float Fertilization { get; set; }
	private Logger _logger;

	public override void _Ready()
	{
		AddToGroup(GameGroup.Soil);
		_logger = new Logger(this);
		WeatherCycle.Instance.WeatherChanged += OnWeatherChanged;
		TimeController.Instance.DayChanged += OnDayChanged;
		TimeController.Instance.NoonOccured += OnNoonOccured;
		_logger.Debug("SoilTile is ready.");
	}

	private void OnNoonOccured()
	{
		var weather = WeatherCycle.Instance.CurrentWeather;
		OnWeatherChanged(weather);
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
				WithdrawHydration(SunnyEvaporationRate);
				break;
			case Weather.Rainy:
				AddWater(RainyHydrationRate);
				break;
			case Weather.Snowy:
				AddWater(SnowyHydrationRate);
				break;
			case Weather.Cloudy:
				WithdrawHydration(CloudyEvaporationRate);
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

		HydrationChanged?.Invoke(Hydration, this);

		return prevHydration - Hydration;
	}

	public void AddWater(float addedWater)
	{
		Hydration = Math.Min(Hydration + addedWater, _maxHydration);
	}
	
	public void PlantPlant(Plant plant)
	{
		Plant = plant;
		Plant.PlantOnTile(this);
	}
}
