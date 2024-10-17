using Godot;
using System.Collections.Generic;
using System.Linq;
using GUI.VendingMachine;
using InventoryV0;

public partial class VendingMachineUI : Control
{
	[Export]
	private Node _itemStackContainer;
	private VendingMachine _vendingMachine;
	private List<ItemStack> _itemStacks;
	public override void _Ready()
	{
		_itemStacks = _itemStackContainer.GetChildren().Cast<ItemStack>().ToList();
	}

	public override void _Process(double delta)
	{
		if (_vendingMachine is null) return;
		
		// i do not know whether this affects performance
		for (var index = 0; index < _itemStacks.Count; index++)
		{
			// TODO: un-uglify this. Thanks!
			// Tedious copying of data from vending machine to UI
			var sourceItem = _vendingMachine.Items[index];
			var destinationItem = _itemStacks[index].InnerItemStack;
			destinationItem.Item = sourceItem.Item;
			destinationItem.Quantity = sourceItem.Quantity;
			_itemStacks[index].InnerItemStack = destinationItem;
		}
	}
}
