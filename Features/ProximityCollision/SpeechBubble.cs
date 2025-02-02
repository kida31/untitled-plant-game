using Godot;
using System;
using System.Threading.Tasks;
using untitledplantgame.Common;

namespace untitledplantgame.ProximityCollision;

public partial class SpeechBubble : Node2D
{
	[Export] private int _timeVisibleMs = 2000;
	
	public async virtual void OnProximityEntered()
	{
		var totalMinutes = (int)(TimeController.Instance.CurrentSeconds / 60);
		var currentDayMinutes = totalMinutes % (24 * 60);

		if (currentDayMinutes is <= 1380 and >= 300)
		{
			return;
		}
		
		Visible = true;
		await Task.Delay(_timeVisibleMs);
		Visible = false;
	}
}
