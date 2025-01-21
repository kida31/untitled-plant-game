using Godot;
using untitledplantgame.Common;
using untitledplantgame.Vending;

namespace untitledplantgame.NPC.NpcTask;

public partial class TriggerVendingMachineResponse : ResponseAction
{
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
		_logger.Debug("VendingMachineOpened");
		EventBus.Instance.BeforeVendingMachineOpen(_vendingMachine);
	}
}
