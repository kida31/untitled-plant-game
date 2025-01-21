using untitledplantgame.Common;
using untitledplantgame.Shops;

namespace untitledplantgame.NPC.NpcTask;

// TODO: Cleanup
public partial class TriggerSeedShopResponse : ResponseAction
{
	// Yes; Storing the seed shop inside the dialogue tree is not a good idea...
	private SeedShop _seedShop;
	private Logger _logger;
	
	public override void _Ready()
	{
		base._Ready();
		_logger = new Logger(this);
		_seedShop = new SeedShop();
		_seedShop.GenerateRandomShopStock();
		
		TimeController.Instance.DayChanged += OnDayChanged;
	}

	public override void ActionAfterResponse()
	{
		_logger.Debug("SeedShop opened.");
		EventBus.Instance.SeedShopOpening(_seedShop);
	}
	
	private void OnDayChanged(int day)
	{
		_seedShop.GenerateRandomShopStock();
	}
}
