using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.ExtensionMethods;
using untitledplantgame.Common.GameStates;
using untitledplantgame.GUI.Items;
using untitledplantgame.Inventory;
using untitledplantgame.Vending;

namespace untitledplantgame.GUI.Vending;

public partial class VendingMachineUI : Control
{
	private const string CoinIconPath = "res://Assets/UI/Book/Icons/CoinIcon.png";
	private const string BBCoin = $"[img=8x8]{CoinIconPath}[/img]";
	[Export] private Node _itemStackContainer;

	[ExportCategory("Emote Bubble")] [Export] private EmoteBubble _emoteBubble;
	[Export] private float _emoteBubbleDuration;
	[Export] private float _fadeInDuration;
	[Export] private float _fadeOutDuration;

	[Export] private Slider _slider;

	[Export] private RichTextLabel _moneyLabel;

	[Export] private Button _withdrawButton;

	[Export] private StorageView _inventoryView;

	private Timer _emoteBubbleTimer;
	private Tween _emoteBubbleTween;
	private VendingMachine _vendingMachine;
	private List<VendingItemView> _itemSlots;
	private Action _inventoryChangedHandler;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);

		// Emote bubble
		_emoteBubbleTimer = new Timer();
		_emoteBubbleTimer.Timeout += OnEmoteBubbleTimeout;
		AddChild(_emoteBubbleTimer);
		_emoteBubble.FadeOut(0f);

		_itemSlots = _itemStackContainer.GetChildren().Cast<VendingItemView>().ToList();
		_slider.ValueChanged += OnSliderValueChanged;

		_withdrawButton.Pressed += () => _vendingMachine.WithdrawGold();

		EventBus.Instance.BeforeVendingMachineOpened += OpenThis;
		this.FadeOut(0);
	}

	public override void _Process(double delta)
	{
		if (_vendingMachine is null)
		{
			return;
		}

		_moneyLabel.Text = $"[center]{_vendingMachine.Gold}{BBCoin}[/center]";
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
		vendingMachine.IsTicking = false; // Pause vending machine while GUI is open

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
		this.FadeIn(0.2f); // TODO: Refactor this in all GUI elements. Seems silly to implement this in every GUI element
		if (GetViewport()?.GuiGetFocusOwner() == null)
		{
			GrabFocus();
		}
	}

	private void CloseThis()
	{
		_vendingMachine.IsTicking = true; // Resume vending machine
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
		
		var tween = this.FadeOut(0.2f);
		ToSignal(tween, Tween.SignalName.Finished).OnCompleted(Hide);
		_logger.Debug("Vending machine closed.");
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

			var price = _vendingMachine.CalculateItemPrice(item) * item.Amount;
			slot.Price = $"[center]{price}{BBCoin}[/center]";
		}
	}

	private void OnSliderValueChanged(double value)
	{
		if (_vendingMachine is null)
		{
			return;
		}

		_vendingMachine.SetPriceSlider((float) value);

		// NOTE: This does not correspond to the actual price or faith, but simply the slider position.
		_emoteBubble.Value = (float) (_slider.Value / _slider.MaxValue);
		_emoteBubbleTween?.Stop();
		_emoteBubbleTween = _emoteBubble.FadeIn(_fadeInDuration);
		_emoteBubbleTimer.Stop();
		_emoteBubbleTimer.Start(_emoteBubbleDuration);
	}

	private void OnEmoteBubbleTimeout()
	{
		_emoteBubbleTween?.Stop();
		_emoteBubbleTween = _emoteBubble.FadeOut(_fadeOutDuration);
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
