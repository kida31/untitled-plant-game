using untitledplantgame.Common;
using untitledplantgame.NPC;

namespace untitledplantgame.Crafting;

public partial class DehydratorInteractable : AInteractable
{
	private Dehydrator _dehydrator;
	private readonly Logger _logger = new("DehydratorInteractable");

	public override void _Ready()
	{
		base._Ready();
		if (_dehydrator == null)
		{
			_dehydrator = new Dehydrator();
		}
	}

	public override void Interact()
	{
		_logger.Debug("Interacted");
		EventBus.Instance.BeforeCraftingStationUiOpen(_dehydrator);
	}
}
