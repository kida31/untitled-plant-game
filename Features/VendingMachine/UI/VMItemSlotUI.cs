using System;
using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.VendingMachine;

public partial class VMItemSlotUI : Control
{
	public event Action Pressed;

	[Export]
	private Label _priceLabel;

	[Export]
	private ItemSlotUI _itemSlot;

	public IItemStack ItemStack
	{
		get => _itemSlot.ItemStack;
		set => SetItemStack(value);
	}

	private float _priceMult = 1.0f;

	public float PriceMult
	{
		get => _priceMult;
		set
		{
			_priceMult = value;
			SetItemStack(ItemStack); // Refresh price
		}
	}

	public override void _Ready()
	{
		_itemSlot.Pressed += () => Pressed?.Invoke();
	}

	private void SetItemStack(IItemStack itemStack)
	{
		_itemSlot.ItemStack = itemStack;
		if (itemStack != null)
		{
			/// <see cref="VendingMachine._priceMultiplier"/>
			var price = Math.Max(1, (int)Math.Ceiling(itemStack.BaseValue * _priceMult));
			_priceLabel.Text = $"{price}g";
		}
		else
		{
			_priceLabel.Text = "";
		}
	}
}
