using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Cycle.Weather;

public enum Weather
{
    Sunny,
    Rainy,
    Snowy,
    Cloudy,
}

[Singleton]
public partial class WeatherCycle : Node
{
    public static WeatherCycle Instance { get; private set; }
    public Action<Weather> WeatherChanged;

    private readonly Logger _logger = new Logger("WeatherCycle");
    private Weather _currentWeather = Weather.Sunny;
    private int _nextChangeAtDay;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Instance != null)
        {
            _logger.Warn("Multiple instances of TimeController found, deleting the new one");
            QueueFree();
            return;
        }

        Instance = this;
        _nextChangeAtDay = GetRandomNumberOfDays();
        _logger.Debug($"First weather change at day {_nextChangeAtDay}");

        TimeController.Instance.DayChanged += ChangeWeatherRandomly;
    }

    /**
     * Returns random int between 3 and 10
     */
    private int GetRandomNumberOfDays(int min = 1, int max = 4)
    {
        var random = new Random();
        return random.Next(min, max); // The upper bound is exclusive, so use 11 to include 10
    }

    /**
     * Changes weather when every 3-10 days
     */
    private void ChangeWeatherRandomly(int day)
    {
        // Check if we need to change the weather
        if (day < _nextChangeAtDay)
        {
            _logger.Debug($"Weather changing at day {_nextChangeAtDay}, today is {day}");
            return;
        }
        
        _logger.Debug("Changing weather...");
        
        // Perform the weather (e.g., change effects, visuals, etc.)
        var newWeather = _currentWeather == Weather.Sunny ? Weather.Rainy : Weather.Sunny;
        WeatherChanged?.Invoke(newWeather);
        _currentWeather = newWeather;

        // Update the last weather change and when the next one should occur
        _nextChangeAtDay = day + GetRandomNumberOfDays();
        _logger.Debug($"Next weather change at day {_nextChangeAtDay}");
    }
}
