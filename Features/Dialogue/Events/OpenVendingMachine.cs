using Godot;
using System;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Vending;

namespace untitledplantgame.Dialogue.Events;

/// <summary>
///		This DialogueEvent should be used for when the player tries to open the VendingMachine via an interaction with the SeedBoy.
/// </summary>
[GlobalClass]
public partial class OpenVendingMachine : DialogueEvent
{
	private VendingMachine _vendingMachine;
	private readonly Logger _logger;

	public OpenVendingMachine()
	{
		_logger = new Logger("OpenVendingMachine");
	}
	
	public override void Execute()
	{
		_vendingMachine = new VendingMachine();

		_logger.Debug("VendingMachine opened.");
		EventBus.Instance.BeforeVendingMachineOpen(_vendingMachine);
	}
}
