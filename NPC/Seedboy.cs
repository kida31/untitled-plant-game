using Godot;

public partial class Seedboy : AbstractNPC
{
	public override void Interact()
	{
		open_shop();
	}

	private void open_shop()
	{
		EventBus.Instance.SeedshopOpened();
	}

	private void close_shop()
	{
		GD.Print("Shop closed");
	}
}
