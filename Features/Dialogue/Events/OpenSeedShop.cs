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
	public readonly SeedShop SeedShop;
	private readonly Logger _logger;

	public OpenSeedShop()
	{
		GD.Print("How often are you called?");
		SeedShop = new SeedShop();
		_logger = new Logger("OpenSeedShop");
		SeedShop.GenerateRandomShopStock();
	}
	
	public override void Execute()
	{
		_logger.Debug("SeedShop opened.");
		EventBus.Instance.SeedShopOpening(SeedShop);
	}
}
