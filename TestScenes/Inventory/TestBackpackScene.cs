using Godot;
using System;
using untitledplantgame.Inventory;

public partial class TestBackpackScene : Node2D
{
	[Export] private BackpackView _backpackView;
	[Export] private Button _addSeedsButton;
	[Export] private Button _addWaterButton;
	[Export] private Button _addSlimeButton;
	[Export] private Button _newBackpackButton;
	[Export] private LineEdit _backpackSizeLineEdit;

	private Backpack _backpack;

	public override void _Ready()
	{
		_backpack = new Backpack(6);
		_backpackView.SetBackpack(_backpack);

		_addSeedsButton!.Pressed += OnAddSeedsButtonOnPressed;
		_addWaterButton!.Pressed += OnAddWaterButtonOnPressed;
		_addSlimeButton!.Pressed += OnAddSlimeButtonOnPressed;
		_newBackpackButton!.Pressed += OnNewBackpackButtonOnPressed;
	}

	private void OnNewBackpackButtonOnPressed()
	{
		var size = _backpackSizeLineEdit.Text.ToInt();
		_backpack = new Backpack(size);
		_backpackView.SetBackpack(_backpack);
	}

	private void OnAddSlimeButtonOnPressed()
	{
		_backpack.Add(new Item("Slime", null));
	}

	private void OnAddWaterButtonOnPressed()
	{
		_backpack.Add(new Item("Water Bottle", null));
	}

	private void OnAddSeedsButtonOnPressed()
	{
		_backpack.Add(new Item("Magic Seeds", null));
	}
}
