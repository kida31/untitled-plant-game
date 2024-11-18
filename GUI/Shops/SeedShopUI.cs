using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.GUI;
using untitledplantgame.Seedshop;

public partial class SeedShopUI : Control
{
	[Export] private Control _slotContainer;
	[Export] private Button _closeButton;
	[Export] private ItemTooltip tooltip;

	private readonly Logger _logger = new("Seedshop");

	private List<ShopSlotUI> _shopSlots;
	private SeedShopShop _currentShop;
	
	public override void _Ready()
	{
		EventBus.Instance.OnSeedshopOpened += OnOpenSeedShop;
		EventBus.Instance.OnSeedshopClosed += HideSeedShop;
		
		_closeButton.Pressed += HideSeedShop;

		_shopSlots = _slotContainer.GetChildren().OfType<ShopSlotUI>().ToList();
		
		_shopSlots.ForEach(slot =>
		{
			var thisSlot = slot; // TODO: Check if currying is needed
			slot.MouseEntered += () => PutTooltip(thisSlot);
			slot.MouseExited += HideTooltip;
			slot.Pressed += () => OnSlotPressed(thisSlot);
		});
	}

	private void OnOpenSeedShop(SeedShopShop shop)
	{
		if (_currentShop != null)
		{
			_currentShop.ShopStockChanged -= SetShopUIContent;
		}
		
		Assertions.AssertTrue(!Visible, "Shop was not supposed to be visible");
		_currentShop = shop;
		SetShopUIContent(shop.CurrentStock.ToList());
		shop.ShopStockChanged += SetShopUIContent;
		Show();
	}

	private void OnSlotPressed(ShopSlotUI thisSlot)
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
		tooltip.Hide();
	}

	private void PutTooltip(ShopSlotUI slot)
	{
		if (slot.ItemStack == null)
		{
			tooltip.Hide();
			return;
		}

		// Set content
		tooltip.ItemStack = slot.ItemStack;
		
		// Set position
		var newPosition = slot.GlobalPosition;
		newPosition.X = slot.GlobalPosition.X + slot.GetRect().Size.X * 0.5f;
		tooltip.GlobalPosition = newPosition;

		tooltip.Show();
	}

	private void SetShopUIContent(List<ItemStack> items)
	{
		for (var i = 0; i < _shopSlots.Count; i++)
		{
			_shopSlots[i].ItemStack = i < items.Count ? items[i] : null;
		}
	}
	
	private void HideSeedShop()
	{
		if (this.Visible)
		{
			this.Hide();
			_logger.Debug("Seedshop closed.");
		}
	}
}
