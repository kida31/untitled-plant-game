using Godot;
using untitledplantgame.Common;
using untitledplantgame.VendingMachine;

public partial class VendingMachineNPC : AbstractNPC
{
	[Export]
	private PackedScene _vendingMachineScene;
	private VendingMachineUI _vendingMachineUi;

	private VendingMachine _vendingMachine;

	private Logger _logger = new Logger("VendingMachineNPC");

	public override void Interact()
	{
		_logger.Debug("Interacted");
		if (_vendingMachine is null)
		{
			_logger.Info("Creating new vending machine");
			// TODO: make serializable/savable vending machine
			_vendingMachine = new VendingMachine();
		}

		if (_vendingMachineUi is null)
		{
			_logger.Info("Creating new vending machine ui");
			_vendingMachineUi = _vendingMachineScene.Instantiate<VendingMachineUI>();
			_vendingMachineUi.SetVendingMachine(_vendingMachine);
		}

		_logger.Debug("Showing vending machine ui");
		// TODO: player state needs to change at some point
		_vendingMachineUi.Show();
	}
}
