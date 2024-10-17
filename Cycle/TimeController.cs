using Godot;
using System;

namespace untitledplantgame.Cycle;

public partial class TimeController : CanvasModulate
{
    private const double MinutesPerDay = 1440;
    private const double MinutesPerHour = 60;
    private const double IngameToRealMinuteDuration = (2 * Math.PI) / MinutesPerDay;

    private double _time;
    private double _colorValue;
    private int _pastDay = -1;
    private int _pastMinute = -1;

    [Export] public GradientTexture1D GradientTexture;
    [Export] public double IngameSpeed = 20.0;

    // The hour with which the day starts
    [Export] public int InitialHour { get; set; } = 12;

    [Signal] public delegate void DayChangedEventHandler(int day);
    [Signal] public delegate void TimeTickEventHandler(int day, int hour, int minute);

    public override void _Ready()
    {
        _time = IngameToRealMinuteDuration * InitialHour * MinutesPerHour;
    }

    /**
     * Called every frame. 'delta' is the elapsed time since the previous frame.
     * _time gets updated every other frame depending on in-game time
     */
    public override void _Process(double delta)
    {
        _time += delta * IngameToRealMinuteDuration * IngameSpeed;
        _colorValue = (Math.Sin(_time - Math.PI / 2) + 1.0) / 2.0;
        this.Color = GradientTexture.Gradient.Sample((float)_colorValue);

        RecalculateTime();
    }

    /**
     * calculates in-game time every time tick (not every frame, as calculated in _Process())
     * emits a signal with the current time
     */
    private void RecalculateTime()
    {
        int totalMinutes = (int)(_time / IngameToRealMinuteDuration);
        
        int day = (int)(totalMinutes / MinutesPerDay);
        int currentDayMinutes = (int)(totalMinutes % MinutesPerDay);
        int hour = (int)(currentDayMinutes / MinutesPerHour);
        int minute = (int)(currentDayMinutes % MinutesPerHour);

        if (_pastDay != day)
        {
            _pastDay = day;
            EmitSignal(SignalName.DayChanged, day);
        }

        if (_pastMinute != minute)
        {
            _pastMinute = minute;
            EmitSignal(SignalName.TimeTick, day, hour, minute);
        }
    }
}
