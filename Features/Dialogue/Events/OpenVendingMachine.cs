using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Vending;

namespace untitledplantgame.Dialogue.Events;

/// <summary>
///		This DialogueEvent should be used for when the player tries to open the VendingMachine via an interaction with the VendingMachine.
/// </summary>
[GlobalClass]
public partial class OpenVendingMachine : DialogueEvent
{
	private readonly Logger _logger = new(nameof(OpenVendingMachine));
	private VendingMachine _vendingMachine;
	private bool _firstTime = true;
	private DialogueResourceObject _tutorialDialogue = GD.Load<DialogueResourceObject>("res://Resources/Dialogue/VendingMachine/VM_firsttime_tutorial.tres");

	public OpenVendingMachine()
	{
		_logger.Info("VendingMachine is null. Creating new instance.");
		_vendingMachine = new VendingMachine();
	}

	public override void Execute()
	{
		_logger.Debug("Triggering BeforeVendingMachineOpen event.");
		EventBus.Instance.BeforeVendingMachineOpen(_vendingMachine);
		
		if (_firstTime)
		{
			_firstTime = false;
			EventBus.Instance.InvokeStartingDialogue(_tutorialDialogue);
		}
	}
}
