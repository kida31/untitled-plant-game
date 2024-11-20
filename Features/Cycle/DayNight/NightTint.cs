using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Cycle.DayNight;

public partial class NightTint : CanvasModulate
{
	/// The gradient texture that represents the day-night cycle
	[Export]
	private GradientTexture1D _gradientTexture;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var colorValue = (Math.Sin(TimeController.Instance.Time - Math.PI / 2) + 1.0) / 2.0;
		Color = _gradientTexture.Gradient.Sample((float)colorValue);
	}
}
