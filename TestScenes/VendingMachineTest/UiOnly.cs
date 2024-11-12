using Godot;
using GUI.VendingMachine;
using untitledplantgame.Inventory.Alt;

public partial class UiOnly : Node2D
{
	[Export] private VendingMachineUI _vendingMachineUi;
	[Export] private Control _inventoryGrid;
	[Export] private PackedScene stackViewTemplate;
	[Export] private Button _sellButton;

	private VendingMachine _vendingMachine;
	private Inventory _inventory;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Init vending machine");
		_vendingMachine = new VendingMachine();
		var inventory = _vendingMachine.Inventory;
		inventory.SetItem(0, new ItemStack("coke", "Coke", null, "This is a coke", ItemCategory.Medicine, 64, 3, 12));
		inventory.SetItem(1, new ItemStack("pepsi", "Pepsi", null, "This is a pepsi", ItemCategory.Medicine, 64, 1, 11));
		_vendingMachineUi.SetVendingMachine(_vendingMachine);

		GD.Print("Init inventory");
		_inventory = new Inventory(15, 64, "PlayerInventory");
		_inventory.SetItem(0, new ItemStack("fanta", "Fanta", null, "This is a fanta", ItemCategory.Medicine, 64, 5, 10));

		GD.Print($"init inventory view s={_inventory.Size}");
		_inventory.GetContents().ForEach(stack =>
		{
			var stackView = stackViewTemplate.Instantiate<ItemSlotUI>();
			GD.Print($"Create {stackView}={stack}");
			stackView.ItemStack = stack;
			_inventoryGrid.AddChild(stackView);
		});

		_sellButton.Pressed += _vendingMachine.SellRandomItems;
	}
}
