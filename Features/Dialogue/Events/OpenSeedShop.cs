using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Shops;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class OpenSeedShop : DialogueEvent
{
	private readonly SeedShop _seedShop;

	public OpenSeedShop()
	{
		_seedShop = new SeedShop();
		_seedShop.GenerateRandomShopStock();
	}
	
	public override void Execute()
	{
		EventBus.Instance.SeedShopOpening(_seedShop);
	}
}
