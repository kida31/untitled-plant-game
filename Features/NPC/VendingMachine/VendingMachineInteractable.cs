using untitledplantgame.Common;
using untitledplantgame.NPC;
using untitledplantgame.Shops;
using untitledplantgame.Vending;

// This namespace is for stuff that should not be talked about (should not be referenced)
namespace untitledplantgame.donottalktome;

public partial class VendingMachineInteractable : AInteractable
{
	private VendingMachine _vendingMachine;
	private readonly Logger _logger = new("VendingMachineNPC");

	public override void _Ready()
	{
		base._Ready();
		_vendingMachine = new VendingMachine();
	}

	public override void Interact()
	{
		_logger.Debug("Interacted");
		EventBus.Instance.BeforeVendingMachineOpen(_vendingMachine);
	}
}
