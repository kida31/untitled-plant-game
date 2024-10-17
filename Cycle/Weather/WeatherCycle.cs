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

public partial class WeatherCycle : Node
{
    private int _lastWeatherChange;
    private int _nextChangeAtDay;
    
    private CanvasLayer _canvasLayer;
    private WeatherCycleUi _ui;
	
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _lastWeatherChange = 0;
        _nextChangeAtDay = GetRandomNumberOfDays();
        
        _canvasLayer = GetNode<CanvasLayer>("CanvasLayer");
        _ui = GetNode<WeatherCycleUi>("CanvasLayer/WeatherCycleUi");

        var timeController = TimeController.Instance;
        timeController.DayChanged += ChangeWeatherRandomly;
    }

    /**
     * Returns random int between 3 and 10
     */
    private int GetRandomNumberOfDays()
    {
        var random = new Random();
        return random.Next(3, 11); // The upper bound is exclusive, so use 11 to include 10
    }

    /**
     * Changes weather when every 3-10 days
     */
    private void ChangeWeatherRandomly(int day)
    {
        // Check if we need to change the weather
        if (day < _lastWeatherChange + _nextChangeAtDay) return;
        
        GD.Print("Changing weather...");

        // Perform the weather (e.g., change effects, visuals, etc.)
        // _ui.ChangeWeather();

        // Update the last weather change and when the next one should occur
        _lastWeatherChange = day;
        _nextChangeAtDay = GetRandomNumberOfDays();
    }
}
