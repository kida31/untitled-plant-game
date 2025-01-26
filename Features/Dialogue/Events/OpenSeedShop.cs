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
	private readonly SeedShop _seedShop;
	private readonly Logger _logger;

	public OpenSeedShop()
	{
		_seedShop = new SeedShop();
		_logger = new Logger("OpenSeedShop");
		_seedShop.GenerateRandomShopStock();
	}
	
	public override void Execute()
	{
		_logger.Debug("SeedShop opened.");
		EventBus.Instance.SeedShopOpening(_seedShop);
	}
}
