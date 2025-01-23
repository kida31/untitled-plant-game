using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class OpenSeedShop : DialogueEvent
{
	public override void Execute()
	{
		EventBus.Instance.SeedshopOpened();
	}
}
