using untitledplantgame.Common;
using untitledplantgame.Shops;

// This namespace is for stuff that should not be talked about (should not be referenced)
namespace untitledplantgame.donottalktome;

public partial class VendingMachineInteractable : AInteractable
{
	private Vending.VendingMachine _vendingMachine;
	private readonly Logger _logger = new ("VendingMachineNPC");

	public override void Interact()
	{
		_logger.Debug("Interacted");
		if (_vendingMachine is null)
		{
			_logger.Info("Creating new vending machine");
			_vendingMachine = new Vending.VendingMachine();
			_logger.Info("Set vending content....");
			_vendingMachine.Inventory.SetContents(new RandomStockGenerator().GetRandomItems(5));
			_logger.Info("Vendign content:" + _vendingMachine.Inventory.GetItems());
		}

		EventBus.Instance.BeforeVendingMachineOpen(_vendingMachine);
	}
}
