using Godot;
using System;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Vending;

namespace untitledplantgame.Dialogue.Events;


public partial class OpenVendingMachine : DialogueEvent
{
	private readonly VendingMachine _vendingMachine = new();
	private readonly Logger _logger = new("OpenVendingMachine");

	public override void Execute()
	{
		_logger.Debug("VendingMachine opened.");
		EventBus.Instance.BeforeVendingMachineOpen(_vendingMachine);
	}
}
