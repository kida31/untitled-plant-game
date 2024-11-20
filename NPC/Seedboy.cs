using Godot;

// TODO: Each NPC should not need to be its own class
// TODO: Refactor to use a single NPC class with a property for the NPC's name and other common properties
public partial class Seedboy : AInteractable
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

	protected override void OnBodyExited(Node body)
	{
		EventBus.Instance.SeedshopClosed();
		base.OnBodyExited(body);
	}
}
