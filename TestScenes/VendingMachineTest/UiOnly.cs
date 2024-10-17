using Godot;
using System;
using InventoryV0;
using untitledplantgame.vending_machine;

public partial class UiOnly : Node2D
{
	[Export] private VendingMachineUI _vendingMachineUi;
	private VendingMachine _vendingMachine;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_vendingMachine = new VendingMachine();
		_vendingMachine.Items[0] =new ItemStack<ISellable>(new ItemImpl("Coke", 3), 8);
		_vendingMachine.Items[1] =new ItemStack<ISellable>(new ItemImpl("Pepsi", 3), 3);
		_vendingMachine.Items[2] =new ItemStack<ISellable>(new ItemImpl("Fanta", 3), 5);
		
		_vendingMachineUi.SetVendingMachine(_vendingMachine);
	}

}
