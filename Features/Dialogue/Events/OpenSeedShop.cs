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
	public SeedShop SeedShop;
	private readonly Logger _logger;

	public OpenSeedShop()
	{
		_logger = new Logger("OpenSeedShop");
		SeedShop = new SeedShop();
		SeedShop.GenerateRandomShopStock();
		TimeController.Instance.DayChanged += UpdateShop;
	}
	
	public override void Execute()
	{
		_logger.Debug("SeedShop opened.");
	
		if (SeedShop == null)
		{
			SeedShop = new SeedShop();
			SeedShop.GenerateRandomShopStock();
		}
		EventBus.Instance.SeedShopOpening(SeedShop);
	}
	
	private void UpdateShop(int day)
	{
		SeedShop.GenerateRandomShopStock();
	}
}
