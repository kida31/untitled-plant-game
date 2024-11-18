using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Items;
using untitledplantgame.VendingMachine;

public partial class ShopSlotUI : Control, IItemSlotUI
{
	public event Action Pressed;

	[Export] private Label _nameLabel;
	[Export] private TextureRect _textureRect;
	[Export] private Label _amountLabel;
	[Export] private Label _priceLabel;

	private ItemStack _itemStack;

	public ItemStack ItemStack
	{
		get => _itemStack;
		set
		{
			_itemStack = value;
			UpdateContent();
		}
	}

	private Logger _logger;
	
	public override void _Ready()
	{
		_logger = new(this);
	}

	private void UpdateContent()
	{
		if (_itemStack == null)
		{
			return;
		}
		
		_nameLabel.Text = _itemStack?.Name ?? "NULL";
		_textureRect.Texture = _itemStack?.Icon ?? _textureRect.Texture;
		_amountLabel.Text = _itemStack?.Amount.ToString() ?? "-";
		_priceLabel.Text = _itemStack?.BaseValue.ToString() ?? "-"; 
	}
}
