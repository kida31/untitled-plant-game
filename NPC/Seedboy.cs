using Godot;
using untitledplantgame.Common;
using untitledplantgame.Shops;

// TODO: Each NPC should not need to be its own class
// TODO: Refactor to use a single NPC class with a property for the NPC's name and other common properties
public partial class Seedboy : AInteractable
{
	private SeedShop _seedShop = new();
	private Logger _logger = new("Seedboy");

	public override void Interact()
	{
		_logger.Debug("Interact()");
		_seedShop.GenerateRandomShopStock();
		EventBus.Instance.SeedshopOpened(_seedShop);
	}

	protected override void OnBodyExited(Node body)
	{
		EventBus.Instance.SeedshopClosed();
		base.OnBodyExited(body);
	}
}
