using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.GUI.Book.Inventories;
using untitledplantgame.GUI.VendingMachine;
using untitledplantgame.Inventory;
using untitledplantgame.VendingMachine;

namespace untitledplantgame.GUI.Vendoring;

public partial class VendingMachineUI : Control
{
	[Export] private Node _itemStackContainer;

	[Export] private EmojiTooltip _emojiTooltip;

	[Export] private Slider _slider;

	[Export] private Label _moneyLabel;

	[Export] private Label _itemNameLabel;

	[Export] private Button _withdrawButton;

	private untitledplantgame.VendingMachine.VendingMachine _vendingMachine;
	private List<VendingItemView> _itemSlots;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		_itemSlots = _itemStackContainer.GetChildren().Cast<VendingItemView>().ToList();
		_slider.ValueChanged += OnSliderValueChanged;

		for (var i = 0; i < _itemSlots.Count; i++)
		{
			var s = _itemSlots[i];
			s.Pressed += GenerateOnPressDelegate(i);
			s.SecondaryPressed += GenerateOnSecondaryPressDelegate(i);
		}

		GetViewport().GuiFocusChanged += OnGuiFocusChanged;
		
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
		if (!IsVisibleInTree()) return;
		if (@event.IsActionPressed(Common.Inputs.GameActions.Book.Back))
		{
			CloseThis();
		}
	}

	private void OpenThis(untitledplantgame.VendingMachine.VendingMachine vendingMachine)
	{
		GameStateMachine.Instance.SetState(GameState.Book);
		SetVendingMachine(vendingMachine);
		Show();
	}

	private void CloseThis()
	{
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
		Hide();
	}

	private void SetVendingMachine(untitledplantgame.VendingMachine.VendingMachine vendingMachine)
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

	private void OnGuiFocusChanged(Control node)
	{
		if (node is InventoryItemView slot)
		{
			_itemNameLabel!.Text = slot.ItemStack?.Name ?? "";
		}
	}

	private Action GenerateOnPressDelegate(int idx)
	{
		return delegate
		{
			if (CursorInventory.Instance is null)
				return;

			if (CursorInventory.Instance.CanClick(_vendingMachine.Inventory, idx))
			{
				CursorInventory.Instance.HandleClick(_vendingMachine.Inventory, idx);
			}

			UpdateContent(_vendingMachine.Inventory);
		};
	}
	
	private Action GenerateOnSecondaryPressDelegate(int idx)
	{
		return delegate
		{
			if (CursorInventory.Instance is null)
				return;

			CursorInventory.Instance.HandleSecondary(_vendingMachine.Inventory, idx);
			UpdateContent(_vendingMachine.Inventory);
		};
	}

	private void OnFaithMultChanged(float obj)
	{
		// TODO:
	}

	private void OnPriceMultChanged(float obj)
	{
		_itemSlots.ForEach(s => s.Price = (int) Math.Ceiling((s.ItemStack?.BaseValue ?? 0) * obj));
	}

	private void UpdateContent(IInventory inventory)
	{
		var items = inventory.GetItems();
		for (var index = 0; index < _itemSlots.Count && index < items.Count; index++)
		{
			_itemSlots[index].UpdateItemView(items[index]);
		}
	}

	private void OnSliderValueChanged(double value)
	{
		if (_vendingMachine is null)
		{
			return;
		}

		_vendingMachine.SetPriceSlider((float) value);

		// Update UI
		var offsetValue = value - _slider.MinValue;
		var offsetPercent = offsetValue / (_slider.MaxValue - _slider.MinValue);

		switch (offsetPercent)
		{
			case > 0.66:
				_emojiTooltip.SetMood(EmojiTooltip.Mood.Sad);
				break;
			case < 0.33:
				_emojiTooltip.SetMood(EmojiTooltip.Mood.Happy);
				break;
			default:
				_emojiTooltip.SetMood(EmojiTooltip.Mood.Neutral);
				break;
		}
	}
}
