using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.GUI.Items;
using untitledplantgame.Inventory;
using untitledplantgame.Vending;

namespace untitledplantgame.GUI.Vending;

public partial class VendingMachineUI : Control
{
	[Export] private Node _itemStackContainer;

	[Export] private EmoteBubble _emoteBubble;

	[Export] private Slider _slider;

	[Export] private Label _moneyLabel;

	[Export] private Button _withdrawButton;

	[Export] private StorageView _inventoryView;

	private VendingMachine _vendingMachine;
	private List<VendingItemView> _itemSlots;
	private Action _inventoryChangedHandler;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		_itemSlots = _itemStackContainer.GetChildren().Cast<VendingItemView>().ToList();
		_slider.ValueChanged += OnSliderValueChanged;

		_withdrawButton.Pressed += () => _vendingMachine.WithdrawGold();

		EventBus.Instance.BeforeVendingMachineOpened += OpenThis;
	}

	public override void _Process(double delta)
	{
		if (_vendingMachine is null)
		{
			return;
		}

		_moneyLabel.Text = "Gold: " + _vendingMachine.Gold;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!IsVisibleInTree()) return; // Do not handle input if not visible
		
		if (@event.IsActionPressed(Common.Inputs.GameActions.Book.Back))
		{
			CloseThis();
		}
	}

	private void OpenThis(VendingMachine vendingMachine)
	{
		GameStateMachine.Instance.SetState(GameState.Book);
		// Set vending
		SetVendingMachine(vendingMachine);

		// Set player inv. This only needs to happen once. Multiple ways to do it.
		// Arbitrary solution: Save the onchange handler, and check whether we have already subscribed
		var playerMedicineInventory = Game.Instance.GetPlayer().Inventory.GetInventory(ItemCategory.Medicine);
		if (_inventoryChangedHandler == null)
		{
			_inventoryChangedHandler = () => _inventoryView.ShowInventory(playerMedicineInventory);
			playerMedicineInventory.InventoryChanged += _inventoryChangedHandler;
		}

		_inventoryView.ShowInventory(playerMedicineInventory);

		Show();
		if (GetViewport()?.GuiGetFocusOwner() == null)
		{
			GrabFocus();
		}
	}
	
	private Action GenerateOnInventoryChanged(IInventory inventory)
	{
		return delegate { _inventoryView.ShowInventory(inventory); };
	}

	private void CloseThis()
	{
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
		Hide();
	}

	private void SetVendingMachine(VendingMachine vendingMachine)
	{
		if (_vendingMachine is not null)
		{
			_vendingMachine.ContentChanged -= UpdateContent;
			_vendingMachine.PriceMultChanged -= OnPriceMultChanged;
			_vendingMachine.FaithMultChanged -= OnFaithMultChanged;
		}

		_vendingMachine = vendingMachine;
		if (_vendingMachine is null)
		{
			return;
		}

		_vendingMachine.ContentChanged += UpdateContent;
		_vendingMachine.PriceMultChanged += OnPriceMultChanged;
		_vendingMachine.FaithMultChanged += OnFaithMultChanged;
		UpdateContent(_vendingMachine.Inventory);
	}

	private void OnFaithMultChanged(float obj)
	{
		// TODO:
	}

	private void OnPriceMultChanged(float obj)
	{
		UpdateItemPriceVisuals();
	}


	private void UpdateContent(IInventory inventory)
	{
		var items = inventory.GetItems();
		for (var index = 0; index < _itemSlots.Count && index < items.Count; index++)
		{
			var itemView = _itemSlots[index];
			itemView.Inventory = inventory;
			itemView.SlotIndex = index;
		}

		UpdateItemPriceVisuals();
	}

	private void UpdateItemPriceVisuals()
	{
		foreach (var slot in _itemSlots)
		{
			var item = slot.ItemStack;
			if (item is null)
			{
				slot.Price = "";
				continue;
			}

			var price = _vendingMachine.CalculateItemPrice(item);
			slot.Price = item.BaseValue == price ? $"{price}g" : $"{price}g ({item.BaseValue}g)";
		}
	}

	private void OnSliderValueChanged(double value)
	{
		if (_vendingMachine is null)
		{
			return;
		}

		_vendingMachine.SetPriceSlider((float) value);
	}

	// Delegates focus to some child  control.
	// This is just called like this so its convenient to find.
	// Would be cool if this could override original GrabFocus.
	public void GrabFocus()
	{
		var first = _itemSlots.FirstOrDefault();
		if (first == null)
		{
			_logger.Warn("No item slots found");
			return;
		}

		first.GrabFocus();
	}
}
