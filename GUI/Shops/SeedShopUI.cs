using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.GUI;
using untitledplantgame.Shops;

public partial class SeedShopUI : Control
{
	// This is the additional offset of the tooltip from the slot.
	// The tooltip itself will alight to the right side of a slot.
	// TODO: Change this once design team has created a design.
	private static readonly Vector2 TooltipOffset = new(8, 0);
	
	[Export]
	private Control _slotContainer;

	[Export]
	private ItemTooltip _tooltip;
	

	private readonly Logger _logger = new("Seedshop");

	private List<ShopSlotUI> _shopSlots;
	private IShop _currentShop;

	public override void _Ready()
	{
		EventBus.Instance.OnSeedshopOpened += OnOpenSeedShop;
		EventBus.Instance.OnSeedshopClosed += HideSeedShop;

		_shopSlots = _slotContainer.GetChildren().OfType<ShopSlotUI>().ToList();

		_shopSlots.ForEach(slot =>
		{
			var thisSlot = slot; // TODO: Check if currying is needed
			slot.MouseEntered += () => PutTooltip(thisSlot);
			slot.MouseExited += HideTooltip;
			slot.Pressed += () => OnSlotPressed(thisSlot);
		});
	}

	private void OnOpenSeedShop(IShop shop)
	{
		if (_currentShop != null)
		{
			_currentShop.ShopStockChanged -= SetShopUIContent;
		}

		Assert.AssertTrue(!Visible, "Shop was not supposed to be visible");
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
		_tooltip.Hide();
	}

	private void PutTooltip(ShopSlotUI slot)
	{
		if (slot.ItemStack == null)
		{
			_tooltip.Hide();
			return;
		}

		// Set content
		_tooltip.ItemStack = slot.ItemStack;

		// Set position
		var newPosition = slot.GlobalPosition;
		newPosition.X += slot.GetRect().Size.X;
		newPosition += TooltipOffset;
		_tooltip.GlobalPosition = newPosition;

		_tooltip.Show();
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
