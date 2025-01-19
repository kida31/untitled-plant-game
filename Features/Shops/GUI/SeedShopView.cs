using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.GUI.Items;
using untitledplantgame.GUI.Shops;
using untitledplantgame.Inventory;
using untitledplantgame.Systems;

namespace untitledplantgame.Shops.GUI;

// TODO: Disallow buying items if player does not have enough space
public partial class SeedShopView : Control
{
	// This is the additional offset of the tooltip from the slot.
	// The tooltip itself will alight to the right side of a slot.
	// TODO: Change this once design team has created a design.
	private static readonly Vector2 TooltipOffset = new(8, 0);

	[Export] private Control _itemContainer;
	[Export] private StorageView _seedInventory;

	private readonly Logger _logger = new("Seedshop");
	private List<ShopItemView> _itemSlots;
	private IShop _currentShop;

	public override void _Ready()
	{
		EventBus.Instance.OnSeedShopOpening += OpenSeedShop;

		_itemSlots = _itemContainer.GetChildren().OfType<ShopItemView>().ToList();

		_itemSlots.ForEach(its => { its.Pressed += () => OnSlotPressed(its); });
	}

	public override void _Process(double delta)
	{
		// Lazy Logic here
		// for each item slot, if player does not have enough money, disable the slot and put grey overlay

		var playerMoney = CurrencyFaithOfficer.Instance.GetCurrentCurrency();
		_itemSlots.ForEach(slot =>
		{
			var item = slot.ItemStack;
			if (item == null)
			{
				return;
			}

			if (item.BaseValue > playerMoney)
			{
				slot.Disabled = true;
				slot.Modulate = new Color(1, 1, 1, 0.5f);
			}
			else
			{
				slot.Disabled = false;
				slot.Modulate = Colors.White;
			}
		});
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!IsVisibleInTree()) return;

		if (@event.IsActionPressed(Shop.CloseShop))
		{
			CloseSeedShop();
		}
	}

	private void OpenSeedShop(IShop shop)
	{
		// Block interaction while shop is open
		GameStateMachine.Instance.SetState(GameState.Shop);
		Assert.AssertTrue(!Visible, "Shop was not supposed to be visible");

		// Setup shop
		if (_currentShop != null)
		{
			_currentShop.ShopStockChanged -= UpdateShopContentVisual;
		}

		_currentShop = shop;
		shop.ShopStockChanged += UpdateShopContentVisual;
		UpdateShopContentVisual(null /* unused */);
		Show();

		// Connect player inventory
		_seedInventory.ShowInventory(Game.Player.Inventory.GetInventory(ItemCategory.Seed));

		// Grab focus
		_itemSlots[0].GrabFocus();
	}

	private void OnSlotPressed(ShopItemView slot)
	{
		var item = slot.ItemStack?.Clone();
		if (item == null)
		{
			return;
		}

		item.Amount = 1;
		_logger.Info("Buy item: " + item);
		_currentShop?.BuyItem(item);
	}

	private void UpdateShopContentVisual(object _)
	{
		var inventory = _currentShop?.Inventory;
		Assert.AssertEquals(_itemSlots.Count, inventory?.Count() ?? 0, "Shop item slots count does not match shop items count");

		if (inventory == null)
		{
			_logger.Info("No inventory found for shop");
			return;
		}

		for (var i = 0; i < _itemSlots.Count; i++)
		{
			var slot = _itemSlots[i];
			slot.Inventory = inventory;
			slot.SlotIndex = i;
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
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
		// Tell subscribers that the shop was closed
		EventBus.Instance.SeedshopClosed();
	}
}
