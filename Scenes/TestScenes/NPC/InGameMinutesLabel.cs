using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Scenes.TestScenes.NPC;

public partial class InGameMinutesLabel : Label
{
	public override void _Ready()
	{
		TimeController.Instance.MinuteTicked += OnMinuteTicked;
	}

	private void OnMinuteTicked(int day, int hour, int minute)
	{
		var minutesTotal = day * 24 * 60 + hour * 60 + minute;
		Text = $"{day}d {hour}h {minute}m ({minutesTotal}m)";
	}
}
