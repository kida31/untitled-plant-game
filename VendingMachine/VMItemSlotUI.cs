using Godot;
using System;
using GUI.VendingMachine;
using untitledplantgame.Inventory.Alt;

public partial class VMItemSlotUI: Control
{
	public event Action Pressed;
	
	[Export] private Label _priceLabel;
	[Export] private ItemSlotUI _itemSlot;
	
	public ItemStack ItemStack
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

	private void SetItemStack(ItemStack itemStack)
	{
		_itemSlot.ItemStack = itemStack;
		_priceLabel.Text = itemStack != null ? $"{itemStack.BaseValue * _priceMult}g" : "";
	}
}
