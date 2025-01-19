using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class GoToBed : DialogueEvent
{
	public override void Execute()
	{
		TimeController.Instance.GoToNextDay();
	}
}
