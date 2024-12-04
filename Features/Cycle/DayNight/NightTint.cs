using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Cycle.DayNight;

public partial class NightTint : CanvasModulate
{
	/// The gradient texture that represents the day-night cycle
	[Export]
	private GradientTexture1D _gradientTexture;

	public override void _Process(double delta)
	{
		// 0 percent is midnight, 0.5 is noon, 1 is midnight again
		var dayPercent = TimeController.Instance.CurrentSeconds / (24f * 60 * 60);
		// We want sin(x=0.5) = 1, sin(x=0) = 0, sin(x=1) = 0
		var colorValue = Math.Abs(Math.Sin(dayPercent * Math.PI));
		Color = _gradientTexture.Gradient.Sample((float)colorValue);
	}
}
