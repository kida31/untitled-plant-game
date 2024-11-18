using Godot;
using untitledplantgame.Seedshop;

public partial class Seedboy : AbstractNPC
{
	private SeedShopShop _shop = new();

	public override void Interact()
	{
		GD.Print("Seedboy interact" + this.ToString());
		_shop.GenerateRandomShopStock();
		EventBus.Instance.SeedShopOpened(_shop);
	}

	protected override void OnBodyExited(Node body)
	{
		EventBus.Instance.SeedshopClosed();
		base.OnBodyExited(body);
	}
}
