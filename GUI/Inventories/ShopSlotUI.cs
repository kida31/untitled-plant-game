using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.VendingMachine;

public partial class ShopSlotUI : Control, IItemSlotUI
{
	public event Action Pressed;

	[Export]
	private Label _nameLabel;

	[Export]
	private TextureRect _textureRect;

	[Export]
	private Label _amountLabel;

	[Export]
	private Label _priceLabel;

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

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
		{
			Pressed?.Invoke();
		}

		if (@event.IsActionPressed("ui_accept"))
		{
			Pressed?.Invoke();
		}
	}

	private void UpdateContent()
	{
		if (_itemStack == null)
		{
			_nameLabel.Text = _itemStack?.Name ?? " ";
			_textureRect.Texture = null;
			_amountLabel.Text = _itemStack?.Amount.ToString() ?? " ";
			_priceLabel.Text = _itemStack?.BaseValue.ToString() ?? " ";
			return;
		}

		_nameLabel.Text = _itemStack.Name;
		_textureRect.Texture = _itemStack.Icon ?? _textureRect.Texture;
		_amountLabel.Text = _itemStack.Amount.ToString();
		_priceLabel.Text = _itemStack.BaseValue.ToString();
	}
}
