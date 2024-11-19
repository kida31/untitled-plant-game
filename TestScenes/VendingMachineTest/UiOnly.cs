using Godot;
using untitledplantgame.Inventory;
using untitledplantgame.VendingMachine;
using ItemSlotUI = untitledplantgame.VendingMachine.ItemSlotUI;

public partial class UiOnly : Node2D
{
	[Export]
	private untitledplantgame.VendingMachine.VendingMachineUI _vendingMachineUi;

	[Export]
	private Control _inventoryGrid;

	[Export]
	private PackedScene stackViewTemplate;

	[Export]
	private Button _sellButton;

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
		_inventory = new Inventory(15, "PlayerInventory");
		_inventory.SetItem(0, new ItemStack("fanta", "Fanta", null, "This is a fanta", ItemCategory.Medicine, 64, 5, 10));

		GD.Print($"init inventory view s={_inventory.Size}");
		foreach (var stack in _inventory)
		{
			var stackView = stackViewTemplate.Instantiate<ItemSlotUI>();
			stackView.Pressed += () => OnInventorySlotPressed(stackView);
			GD.Print($"Create {stackView}={stack}");
			_inventoryGrid.AddChild(stackView);
			stackView.ItemStack = stack;
		}

		_sellButton.Pressed += _vendingMachine.SellRandomItems;
	}

	private void OnInventorySlotPressed(ItemSlotUI slot)
	{
		if (untitledplantgame.VendingMachine.CursorFriend.Instance is null)
			return;

		var idx = _inventory.GetContents().IndexOf(slot.ItemStack);
		if (idx == -1)
		{
			GD.PrintErr("Unexpected index");
		}

		if (untitledplantgame.VendingMachine.CursorFriend.Instance.ItemStack == null)
		{
			// Empty hand
			var item = _inventory.GetItem(idx);
			if (item == null)
				return;
			untitledplantgame.VendingMachine.CursorFriend.Instance.ItemStack = item;
			_inventory.SetItem(idx, null);
		}
		else
		{
			// Holding item
			if (slot.ItemStack == null)
			{
				// Empty inventory slot
				_inventory.SetItem(idx, untitledplantgame.VendingMachine.CursorFriend.Instance.ItemStack);
				untitledplantgame.VendingMachine.CursorFriend.Instance.ItemStack = null;
				GD.Print("Dropped item in inventory");
			}
			else
			{
				// TODO: may need to stack instead
				// Swap
				var temp = _inventory.GetItem(idx);
				_inventory.SetItem(idx, untitledplantgame.VendingMachine.CursorFriend.Instance.ItemStack);
				untitledplantgame.VendingMachine.CursorFriend.Instance.ItemStack = temp;
			}
		}

		// Update ui
		slot.ItemStack = _inventory.GetItem(idx);
	}
}
