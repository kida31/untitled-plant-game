using Godot;
using untitledplantgame.Shops;

public partial class Seedboy : AbstractNPC
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
