using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Shops;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class OpenSeedShop : DialogueEvent
{
	private SeedShop _seedShop;
	public override void Execute()
	{
		_seedShop = new SeedShop();
		_seedShop.GenerateRandomShopStock();
		EventBus.Instance.SeedShopOpening(_seedShop);
	}
}
