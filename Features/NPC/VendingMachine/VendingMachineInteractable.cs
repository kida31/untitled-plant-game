using Godot;
using System;
using untitledplantgame.Common;

// This namespace is for stuff that should not be talked about (should not be referenced)
namespace untitledplantgame.donottalktome;

public partial class VendingMachineInteractable : AInteractable
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

	// public override void _Ready()
	// {
	// 	EventBus.Instance.OnSceneChange += OnSceneChange;
	// }

	// public override void _ExitTree()
	// {
	// 	EventBus.Instance.OnSceneChange -= OnSceneChange;
	// }

	// private void OnSceneChange(Node from, Node to)
	// {
	// 	_logger.Debug("Scene changed from " + from.Name + " to " + to.Name);
	// 	// _vendingMachine = null;
	// }
}
