using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.Inventory;

namespace untitledplantgame.VendingMachine;

public partial class VendingMachineUI : Control
{
	[Export]
	private Node _itemStackContainer;

	[Export]
	private EmojiTooltip _emojiTooltip;

	[Export]
	private Slider _slider;

	[Export]
	private Label _moneyLabel;

	[Export]
	private Label _itemNameLabel;

	[Export]
	private Button _withdrawButton;

	private VendingMachine _vendingMachine;
	private List<VMItemSlotUI> _itemSlots;

	public override void _Ready()
	{
		_itemSlots = _itemStackContainer.GetChildren().Cast<VMItemSlotUI>().ToList();
		_slider.ValueChanged += OnSliderValueChanged;

		for (var i = 0; i < _itemSlots.Count; i++)
		{
			var s = _itemSlots[i];
			s.Pressed += OnItemSlotPressedCurry(i);
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
		if (@event.IsActionPressed(Book.Back))
		{
			CloseThis();
		}
	}

	private void OpenThis(VendingMachine vendingMachine)
	{
		GameStateMachine.Instance.SetState(GameState.Book);
		SetVendingMachine(vendingMachine);
		Show();
	}

	private void CloseThis()
	{
		GameStateMachine.Instance.RevertState();
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

	private void OnGuiFocusChanged(Control node)
	{
		if (node is ItemSlotUI slot)
		{
			_itemNameLabel!.Text = slot.ItemStack?.Name ?? "";
		}
	}

	private Action OnItemSlotPressedCurry(int idx)
	{
		return () =>
		{
			if (CursorFriend.Instance is null)
				return;

			if (CursorFriend.Instance.ItemStack == null)
			{
				// Empty hand
				var item = _vendingMachine.Inventory.GetItem(idx);
				if (item == null)
					return;
				CursorFriend.Instance.ItemStack = item;
				_vendingMachine.Inventory.SetItem(idx, null);
			}
			else
			{
				// Holding item
				if (_itemSlots[idx].ItemStack == null)
				{
					// Empty vending machine slot
					_vendingMachine.Inventory.SetItem(idx, CursorFriend.Instance.ItemStack);
					CursorFriend.Instance.ItemStack = null;
				}
				else
				{
					// TODO: may need to stack instead
					// Swap
					var temp = _vendingMachine.Inventory.GetItem(idx);
					_vendingMachine.Inventory.SetItem(idx, CursorFriend.Instance.ItemStack);
					CursorFriend.Instance.ItemStack = temp;
				}
			}
			UpdateContent(_vendingMachine.Inventory);
		};
	}

	private void OnFaithMultChanged(float obj)
	{
		// TODO:
	}

	private void OnPriceMultChanged(float obj)
	{
		_itemSlots.ForEach(s => s.PriceMult = obj);
	}

	private void UpdateContent(IInventory inventory)
	{
		var items = inventory.GetContents();
		for (var index = 0; index < _itemSlots.Count && index < items.Count; index++)
		{
			_itemSlots[index].ItemStack = items[index];
		}
	}

	private void OnSliderValueChanged(double value)
	{
		if (_vendingMachine is null)
		{
			return;
		}

		_vendingMachine.SetPriceSlider((float)value);

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
