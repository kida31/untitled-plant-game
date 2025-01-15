using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.VendingMachine;
using VendingMachineUI = untitledplantgame.GUI.Vendoring.VendingMachineUI;

public partial class UiOnly : Node2D
{
	[Export]
	private VendingMachineUI _vendingMachineUi;

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
		inventory.SetItem(0, new ItemStack("coke")
		{
			Name = "Coke",
			ToolTipDescription = "This is a coke",
			BaseValue = 3,
			Amount = 12,
			MaxStackSize = 64,
		});
		inventory.SetItem(1, new ItemStack("pepsi")
		{
			Name = "Pepsi",
			ToolTipDescription = "This is a pepsi",
			BaseValue = 1,
			Amount = 11,
			MaxStackSize = 64,
		});

		EventBus.Instance.BeforeVendingMachineOpen(_vendingMachine);

		GD.Print("Init inventory");
		_inventory = new Inventory(15, "PlayerInventory");
		inventory.SetItem(0, new ItemStack("fanta")
		{
			Name = "Fanta",
			ToolTipDescription = "This is a fanta",
			BaseValue = 5,
			Amount = 10,
			MaxStackSize = 64,
		});

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
		if (CursorFriend.Instance is null)
			return;

		var idx = _inventory.GetItems().IndexOf(slot.ItemStack);
		if (idx == -1)
		{
			GD.PrintErr("Unexpected index");
		}

		if (CursorFriend.Instance.ItemStack == null)
		{
			// Empty hand
			var item = _inventory.GetItem(idx);
			if (item == null)
				return;
			CursorFriend.Instance.ItemStack = item;
			_inventory.SetItem(idx, null);
		}
		else
		{
			// Holding item
			if (slot.ItemStack == null)
			{
				// Empty inventory slot
				_inventory.SetItem(idx, CursorFriend.Instance.ItemStack);
				CursorFriend.Instance.ItemStack = null;
				GD.Print("Dropped item in inventory");
			}
			else
			{
				// TODO: may need to stack instead
				// Swap
				var temp = _inventory.GetItem(idx);
				_inventory.SetItem(idx, CursorFriend.Instance.ItemStack);
				CursorFriend.Instance.ItemStack = temp;
			}
		}

		// Update ui
		slot.ItemStack = _inventory.GetItem(idx);
	}
}
