using Godot;
using System;
using InventoryV0;
using untitledplantgame.vending_machine;
using GUI.VendingMachine;
using System.Linq;
using System.Collections.Generic;

public partial class UiOnly : Node2D
{
	[Export] private VendingMachineUI _vendingMachineUi;
	[Export] private Control _inventoryGrid;
	[Export] private PackedScene stackViewTemplate;
	private VendingMachine _vendingMachine;
	private List<ItemStack<IStorable>> _inventory;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Init vending machine");
		_vendingMachine = new VendingMachine();
		_vendingMachine.Items[0] = new ItemStack<ISellable>(new ItemImpl("Coke", 3), 8);
		_vendingMachine.Items[1] = new ItemStack<ISellable>(new ItemImpl("Pepsi", 3), 3);
		_vendingMachineUi.SetVendingMachine(_vendingMachine);
		
		GD.Print("Init inventory");
		_inventory = new(new ItemStack<IStorable>[8]);
		_inventory[0] = new ItemStack<IStorable>(new ItemImpl("Fanta", 3), 5);

		GD.Print($"init inventory view s={_inventory.Count}");
		_inventory.ForEach((stack) => {
			var stackView = stackViewTemplate.Instantiate<ItemStackView>();
			GD.Print($"Create {stackView}={stack}");
			stackView.InnerItemStack = stack;
			_inventoryGrid.AddChild(stackView);
		});
	}

}
