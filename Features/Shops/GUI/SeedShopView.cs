using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.GUI.Items;
using untitledplantgame.GUI.Shops;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.GUI;

namespace untitledplantgame.Shops.GUI;

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

		_itemSlots.ForEach(its =>
		{
			its.Pressed += () => OnSlotPressed(its);
		});

		// // Adjust navigation, hacky
		// var columnCount = (_slotContainer as GridContainer)!.Columns;
		// for (var i = 0; i < _shopSlots.Count; i++)
		// {
		// 	var slot = _shopSlots[i];
		// 	slot.FocusMode = FocusModeEnum.All;
		// 	// RightNeighbour, Not last column
		// 	if (i % columnCount != columnCount - 1)
		// 	{
		// 		slot.FocusNeighborRight = _shopSlots[i + 1].GetPath();
		// 	}
		// 	// LeftNeighbour, Not first column
		// 	if (i % columnCount != 0)
		// 	{
		// 		slot.FocusNeighborLeft = _shopSlots[i - 1].GetPath();
		// 	}
		// 	// TopNeighbour, Not first row
		// 	if (i >= columnCount)
		// 	{
		// 		slot.FocusNeighborTop = _shopSlots[i - columnCount].GetPath();
		// 	}
		// 	// BottomNeighbour, Not last row
		// 	if (i < _shopSlots.Count - columnCount)
		// 	{
		// 		slot.FocusNeighborBottom = _shopSlots[i + columnCount].GetPath();
		// 	}
		// }
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
