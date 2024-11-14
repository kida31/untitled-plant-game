using Godot;

public partial class Seedboy : AbstractNPC
{
	private CanvasLayer Seedshop;

	public override void Interact()
	{
		open_shop();
	}

	private void open_shop()
	{
		EventBus.Instance.SeedshopOpened();
	}

	private new void OnBodyExited(Node body)
	{
		EventBus.Instance.SeedshopClosed();
		base.OnBodyExited(body);
	}
}
