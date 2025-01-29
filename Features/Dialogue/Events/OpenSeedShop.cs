using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Shops;

namespace untitledplantgame.Dialogue.Events;

/// <summary>
///		This DialogueEvent should be used for when the player tries to open the SeedShop via an interaction with the SeedBoy.
/// </summary>
[GlobalClass]
public partial class OpenSeedShop : DialogueEvent
{
	private SeedShop _seedShop;
	
	private void UpdateShop(int day)
	{
		_seedShop.GenerateRandomShopStock();
	}

	public override void Execute()
	{
		if (_seedShop == null)
		{
			_seedShop = new SeedShop();
			_seedShop.GenerateRandomShopStock();
			TimeController.Instance.DayChanged += UpdateShop;
		}
		EventBus.Instance.SeedShopOpening(_seedShop);
	}
}
