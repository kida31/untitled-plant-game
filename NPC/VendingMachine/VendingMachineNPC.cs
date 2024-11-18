using Godot;
using System;
using untitledplantgame.Common;
using untitledplantgame.VendingMachine;

public partial class VendingMachineNPC : Area2D, IInteractable
{
	[Export] private PackedScene _vendingMachineScene;
	private untitledplantgame.VendingMachine.VendingMachineUI _vendingMachineUi;
	
	private VendingMachine _vendingMachine;
	
	private Logger _logger = new Logger("VendingMachineNPC");

	public override void _Ready()
	{
		AddToGroup("Interactables");
	}

	public void Interact()
	{
		_logger.Debug("Interacted");
		if (_vendingMachine is null)
		{
			_logger.Info("Creating new vending machine");
			// TODO: make serializable/saveable vending machine
			_vendingMachine = new VendingMachine();
		}
		
		if (_vendingMachineUi is null)
		{
			_logger.Info("Creating new vending machine ui");
			_vendingMachineUi = _vendingMachineScene.Instantiate<untitledplantgame.VendingMachine.VendingMachineUI>();
			_vendingMachineUi.SetVendingMachine(_vendingMachine);
		}
		
		_logger.Debug("Showing vending machine ui");
		// TODO: player state needs to change at some point
		_vendingMachineUi.Show();
	}

	public Vector2 GetGlobalInteractablePosition()
	{
		return GlobalPosition;
	}
	
}
