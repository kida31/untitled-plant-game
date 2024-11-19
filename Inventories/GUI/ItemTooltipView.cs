﻿using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Inventory.GUI;

public partial class ItemTooltipView : Control
{
	[Export]
	private Label _nameLabel;

	[Export]
	private Label _descriptionLabel;

	public ItemStack ItemStack
	{
		get => _itemStack;
		set
		{
			_itemStack = value;
			UpdateContent();
		}
	}

	private ItemStack _itemStack;

	private void UpdateContent()
	{
		_nameLabel.Text = _itemStack?.Name ?? "ITEMNAME";
		_descriptionLabel.Text = _itemStack?.Description ?? "DESCRIPTION";
	}
}
