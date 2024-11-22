using Godot;
using untitledplantgame.Shops;

// TODO: Each NPC should not need to be its own class
// TODO: Refactor to use a single NPC class with a property for the NPC's name and other common properties
public partial class SeedBoy : AInteractable
{
	private SeedShop _seedShop;

	public override void _Ready()
	{
		base._Ready();
		_seedShop = new SeedShop();
		_seedShop.GenerateRandomShopStock();
	}

	public override void Interact()
	{
		EventBus.Instance.SeedShopOpening(_seedShop);
	}

	protected override void OnBodyExited(Node body)
	{
		base.OnBodyExited(body);
		EventBus.Instance.SeedshopClosed();
	}
}
