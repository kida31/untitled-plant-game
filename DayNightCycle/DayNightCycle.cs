using Godot;
using System;

namespace untitledplantgame.DayNightCycle;


public partial class DayNightCycle : CanvasModulate
{
	private const double MinutesPerDay = 1440;
	private const double MinutesPerHour = 60;
	private const double IngameToRealMinuteDuration = (2 * Math.PI) / MinutesPerDay;
	
	
	
	private double _time;
	private double _colorValue;
	[Export] public GradientTexture1D GradientTexture;

	public override void _Ready()
	{
		GradientTexture = GetNode<GradientTexture1D>("res://DayNightCycle/daynightcycle-gradient-texture.tres");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_time += delta;
		_colorValue = (Math.Sin(_time - Math.PI / 2) + 1.0) / 2.0;
		this.Color = GradientTexture.Gradient.Sample((float)_colorValue);
		
		RecalculateTime();
	}

	private void RecalculateTime()
	{
		int totalMinutes = (int)(_time / IngameToRealMinuteDuration);

		int day = (int) (totalMinutes / MinutesPerDay);
		int currentDayMinutes = (int) (totalMinutes % MinutesPerDay);
		int hour = (int)(currentDayMinutes / MinutesPerHour);
		int minute = (int)(currentDayMinutes % MinutesPerHour);
		
	}
}


