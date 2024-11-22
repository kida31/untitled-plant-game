using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.GUI;

namespace untitledplantgame.Shops.GUI;

public partial class SeedShopView : Control
{
	// This is the additional offset of the tooltip from the slot.
	// The tooltip itself will alight to the right side of a slot.
	// TODO: Change this once design team has created a design.
	private static readonly Vector2 TooltipOffset = new(8, 0);

	[Export]
	private Control _slotContainer;

	[Export]
	private ItemTooltipView _tooltipView;

	private readonly Logger _logger = new("Seedshop");

	private List<ShopItemStackView> _shopSlots;
	private IShop _currentShop;

	public override void _Ready()
	{
		EventBus.Instance.OnSeedShopOpening += OpenSeedShop;

		_shopSlots = _slotContainer.GetChildren().OfType<ShopItemStackView>().ToList();

		_shopSlots.ForEach(slot =>
		{
			var thisSlot = slot; // TODO: Check if currying is needed
			slot.MouseEntered += () => PutTooltip(thisSlot);
			slot.MouseExited += HideTooltip;
			slot.FocusEntered += () => PutTooltip(thisSlot);
			slot.FocusExited += HideTooltip;
			slot.Pressed += () => OnSlotPressed(thisSlot);
		});

		// Adjust navigation, hacky
		var columnCount = (_slotContainer as GridContainer)!.Columns;
		for (var i = 0; i < _shopSlots.Count; i++)
		{
			var slot = _shopSlots[i];
			slot.FocusMode = FocusModeEnum.All;
			// RightNeighbour, Not last column
			if (i % columnCount != columnCount - 1)
			{
				slot.FocusNeighborRight = _shopSlots[i + 1].GetPath();
			}
			// LeftNeighbour, Not first column
			if (i % columnCount != 0)
			{
				slot.FocusNeighborLeft = _shopSlots[i - 1].GetPath();
			}
			// TopNeighbour, Not first row
			if (i >= columnCount)
			{
				slot.FocusNeighborTop = _shopSlots[i - columnCount].GetPath();
			}
			// BottomNeighbour, Not last row
			if (i < _shopSlots.Count - columnCount)
			{
				slot.FocusNeighborBottom = _shopSlots[i + columnCount].GetPath();
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		base._UnhandledInput(@event);
		if (@event.IsActionPressed(Shop.CloseShop))
		{
			CloseSeedShop();
		}
	}

	private void OpenSeedShop(IShop shop)
	{
		if (_currentShop != null)
		{
			_currentShop.ShopStockChanged -= SetShopUIContent;
		}

		// Block interaction while shop is open
		GameStateMachine.Instance.SetState(GameState.Shop);

		Assert.AssertTrue(!Visible, "Shop was not supposed to be visible");
		_currentShop = shop;
		SetShopUIContent(shop.CurrentStock.ToList());
		shop.ShopStockChanged += SetShopUIContent;
		Show();

		// Grab focus
		_shopSlots[0].GrabFocus();
	}

	private void OnSlotPressed(ShopItemStackView thisSlot)
	{
		var item = thisSlot.ItemStack?.Clone() as ItemStack;
		if (item == null)
		{
			return;
		}
		item.Amount = 1;
		_logger.Info("Buy item: " + item);
		_currentShop?.BuyItem(item);
	}

	private void HideTooltip()
	{
		_tooltipView.Hide();
	}

	private void PutTooltip(ShopItemStackView slot)
	{
		if (slot.ItemStack == null)
		{
			_tooltipView.Hide();
			return;
		}

		// Set content
		_tooltipView.ItemStack = slot.ItemStack;

		// Set position
		var newPosition = slot.GlobalPosition;
		newPosition.X += slot.GetRect().Size.X;
		newPosition += TooltipOffset;
		_tooltipView.GlobalPosition = newPosition;

		_tooltipView.Show();
	}

	private void SetShopUIContent(List<ItemStack> items)
	{
		for (var i = 0; i < _shopSlots.Count; i++)
		{
			_shopSlots[i].ItemStack = i < items.Count ? items[i] : null;
		}
	}

	private void CloseSeedShop()
	{
		if (this.Visible)
		{
			this.Hide();
			_logger.Debug("Seedshop closed.");
		}

		// Change game state back to previous state
		GameStateMachine.Instance.RevertState();
		// Tell subscribers that the shop was closed
		EventBus.Instance.SeedshopClosed();
	}
}
