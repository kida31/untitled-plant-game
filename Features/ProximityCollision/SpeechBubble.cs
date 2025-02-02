using Godot;
using System;
using System.Threading.Tasks;
using untitledplantgame.Common;
using untitledplantgame.Common.ExtensionMethods;

namespace untitledplantgame.ProximityCollision;

public partial class SpeechBubble : Node2D
{
	[Export] private int _timeVisibleMs = 2000;

	public override void _Ready()
	{
		Show();
		this.FadeOut(0);
	}

	public async virtual void OnProximityEntered()
	{
		var totalMinutes = (int)(TimeController.Instance.CurrentSeconds / 60);
		var currentDayMinutes = totalMinutes % (24 * 60);

		if (currentDayMinutes is <= 1380 and >= 300)
		{
			return;
		}

		this.FadeIn(0.4f);
		await Task.Delay(_timeVisibleMs);
		this.FadeOut(0.4f);
	}
}
