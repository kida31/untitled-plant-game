using Godot;
using untitledplantgame.Common;
using untitledplantgame.Vending;

namespace untitledplantgame.NPC.NpcTask;

// TODO: Cleanup
public partial class TriggerVendingMachineResponse : ResponseAction
{
	// Yes; Storing the vending machine inside the dialogue tree is not a good idea...
	private VendingMachine _vendingMachine;
	private Logger _logger;
	
	public override void _Ready()
	{
		base._Ready();
		_logger = new Logger(this);
		_vendingMachine = new VendingMachine();
	}
	
	public override void ActionAfterResponse()
	{
		_logger.Debug("VendingMachine opened.");
		EventBus.Instance.BeforeVendingMachineOpen(_vendingMachine);
	}
}
