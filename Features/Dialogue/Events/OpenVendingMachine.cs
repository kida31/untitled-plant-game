﻿using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Vending;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class OpenVendingMachine : DialogueEvent
{
	private readonly Logger _logger = new(nameof(OpenVendingMachine));
	private VendingMachine _vendingMachine;

	public override void Execute()
	{
		if (_vendingMachine == null)
		{
			_logger.Info("VendingMachine is null. Creating new instance.");
			_vendingMachine = new VendingMachine();
		}

		_logger.Debug("Triggering BeforeVendingMachineOpen event.");
		EventBus.Instance.BeforeVendingMachineOpen(_vendingMachine);
	}
}