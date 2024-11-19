using untitledplantgame.Common;

// This namespace is for stuff that should not be talked about (should not be referenced)
namespace untitledplantgame.donottalktome;

public partial class VendingMachineInteractable : AbstractNPC
{
	private VendingMachine.VendingMachine _vendingMachine;
	private Logger _logger = new Logger("VendingMachineNPC");

	public override void Interact()
	{
		_logger.Debug("Interacted");
		if (_vendingMachine is null)
		{
			_logger.Info("Creating new vending machine");
			_vendingMachine = new VendingMachine.VendingMachine();
		}
		
		EventBus.Instance.BeforeVendingMachineOpen(_vendingMachine);
	}
}
