using Godot;
using Godot.Collections;
using untitledplantgame.Common;

namespace untitledplantgame.Cycle.Weather;

public partial class WeatherCycleUi : Node
{
	private readonly Logger _logger = new("WeatherCycleUi");
	private GpuParticles2D _gardenRain;
	private GpuParticles2D _pierRain;
	

	public override void _Ready()
	{
		_gardenRain = GetNode<GpuParticles2D>("RainParticlesGarden");
		_pierRain = GetNode<GpuParticles2D>("RainParticlesPier");
		_gardenRain.Visible = true;
		_pierRain.Visible = true;
		_logger.Debug("Starting a rainy day 🌧️️"); //TODO: change back to start with sunny day

		WeatherCycle.Instance.WeatherChanged += ChangeWeather;
	}

	private void ChangeWeather(Weather newWeather)
	{
		switch (newWeather)
		{
			case Weather.Sunny:
				_gardenRain.Visible = false;
				_pierRain.Visible = false;
				_logger.Debug("Sunshine ☀️");
				break;
			case Weather.Rainy:
				_gardenRain.Visible = true;
				_pierRain.Visible = true;
				_logger.Debug("It's raining main, hallelujah! 🌧️");
				break;
			default:
				_logger.Error("Weather isn't supported by the GUI yet");
				break;
		}
	}
}
