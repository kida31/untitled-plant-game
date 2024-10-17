using Godot;

namespace untitledplantgame.DayNightCycle.UI;

public partial class WeatherCycleUi : Control
{
    private Weather _currentWeather = Weather.Sunny;
    private GpuParticles2D _rainOverlay;

    public override void _Ready()
    {
        _rainOverlay = GetNode<GpuParticles2D>("Sprite2D/RainParticles");
        _rainOverlay.Visible = false;
    }

    public void ChangeWeather()
    {
        switch (_currentWeather)
        {
            case Weather.Sunny:
                MakeItRainy();
                break;
            case Weather.Rainy:
                MakeItSunny();
                break;
            default:
                GD.PrintErr("Weather " + _currentWeather + " is not supported yet");
                break;
        }
    }

    public Weather CurrentWeather()
    {
        return _currentWeather;
    }
    
    private void MakeItSunny()
    {
        _currentWeather = Weather.Sunny;
        _rainOverlay.Visible = false;
    }

    private void MakeItRainy()
    {
        _currentWeather = Weather.Rainy;
        _rainOverlay.Visible = true;
    }
}
