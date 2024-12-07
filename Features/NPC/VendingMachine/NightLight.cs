using Godot;
using untitledplantgame.Common;

public partial class NightLight : PointLight2D
{
	private bool _isOn = false;

	public override void _Ready()
	{
		TimeController.Instance.MinuteTicked += OnMinuteTicked;
	}

	private void OnMinuteTicked(int day, int hour, int minute)
	{
		if (hour >= 20 || hour < 6)
		{
			Visible = true;
		}
		else
		{
			Visible = false;
		}
	}
}
