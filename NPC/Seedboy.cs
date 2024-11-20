using Godot;
using untitledplantgame.Shops;

// TODO: Each NPC should not need to be its own class
// TODO: Refactor to use a single NPC class with a property for the NPC's name and other common properties
public partial class Seedboy : AInteractable
{
	private SeedShop _seedShop = new();

	public override void Interact()
	{
		GD.Print("Seedboy interact" + this.ToString());
		_seedShop.GenerateRandomShopStock();
		EventBus.Instance.SeedShopOpened(_seedShop);
	}

	protected override void OnBodyExited(Node body)
	{
		EventBus.Instance.SeedshopClosed();
		base.OnBodyExited(body);
	}
}
