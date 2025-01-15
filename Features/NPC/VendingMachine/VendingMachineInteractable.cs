using untitledplantgame.Common;
using untitledplantgame.Shops;

// This namespace is for stuff that should not be talked about (should not be referenced)
namespace untitledplantgame.donottalktome;

public partial class VendingMachineInteractable : AInteractable
{
	private VendingMachine.VendingMachine _vendingMachine;
	private readonly Logger _logger = new ("VendingMachineNPC");

	public override void Interact()
	{
		_logger.Debug("Interacted");
		if (_vendingMachine is null)
		{
			_logger.Info("Creating new vending machine");
			_vendingMachine = new VendingMachine.VendingMachine();
			_vendingMachine.Inventory.SetContents(new RandomStockGenerator().GetRandomItems(5));
		}

		EventBus.Instance.BeforeVendingMachineOpen(_vendingMachine);
	}
}
