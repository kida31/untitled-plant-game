using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.VendingMachine;

public partial class ShopSlot : Panel, IItemSlotUI
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
		_nameLabel.Text = _itemStack.Name;
		_textureRect.Texture = _itemStack.Icon ?? _textureRect.Texture;
		_amountLabel.Text = _itemStack.Amount.ToString();
		_priceLabel.Text = _itemStack.BaseValue.ToString();
	}

	private void OnMouseEntered()
	{
		Tooltip tooltipInstance = null;
		
		tooltipInstance.slot = _nameLabel.Text;
		float x = GlobalPosition.X + GetRect().Size.X;
		float y = GlobalPosition.Y;
		Vector2I position = (Vector2I) new Vector2(x, y);
		tooltipInstance.Position = position;
		tooltipInstance.Transparent = true;

		AddChild(tooltipInstance);
		// await ToSignal(GetTree().CreateTimer(5.0f), "timeout");
		if (HasNode("Tooltip") && tooltipInstance.valid)
		{
			tooltipInstance.Show();
		}
	}

	private void OnMouseExited()
	{
		if (HasNode("Tooltip"))
		{
			GetNode("Tooltip").QueueFree();
			_logger.Debug("Tooltip removed");
		}
		else
		{
			_logger.Debug("Tooltip node not found");
		}
	}
}
