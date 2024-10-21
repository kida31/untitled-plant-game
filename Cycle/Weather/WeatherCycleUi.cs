using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Cycle.Weather;

public partial class WeatherCycleUi : Node
{
	private readonly Logger _logger = new("WeatherCycleUi");
	private GpuParticles2D _rainOverlay;

	public override void _Ready()
	{
		_rainOverlay = GetNode<GpuParticles2D>("RainParticles");
		_rainOverlay.Visible = false;
		_logger.Debug("Starting a sunny day ☀️");

		WeatherCycle.Instance.WeatherChanged += ChangeWeather;
	}

	private void ChangeWeather(Weather newWeather)
	{
		switch (newWeather)
		{
			case Weather.Sunny:
				_rainOverlay.Visible = false;
				_logger.Debug("Sunshine ☀️");
				break;
			case Weather.Rainy:
				_rainOverlay.Visible = true;
				_logger.Debug("It's raining main, hallelujah!");
				break;
			default:
				_logger.Error("Weather isn't supported by the GUI yet");
				break;
		}
	}
}
