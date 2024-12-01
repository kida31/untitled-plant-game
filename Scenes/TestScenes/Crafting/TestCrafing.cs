using Godot;
using System;
using untitledplantgame.Crafting;
using untitledplantgame.Inventory;

public partial class TestCrafing : Node2D
{
	[Export] private DehydratorUi _dehydratorUi;
	[Export] private Button _openDehydratorButton;
	[Export] private Button _addItemButton;
	[Export] private Button _removeItemButton;

	private ItemStack _testItem;
	private Dehydrator _dehydrator;
	private int _index;

	public override void _Ready()
	{
		var texture = GD.Load<Texture2D>("res://Assets/OverworldAssets/Plant/DeadPlant.png");
		_testItem = new ItemStack("item_id", "Dead Plants", texture,
			"A dead plant", ItemCategory.Plant, 1, 1);
		_dehydrator = new Dehydrator();
		_index = -1;
		
		_openDehydratorButton.Pressed += OpenDehydrator;
		_addItemButton.Pressed += AddItem;
		_removeItemButton.Pressed += RemoveItem;
	}

	public override void _Process(double delta)
	{
		_dehydrator.Process(delta);
	}

	private void RemoveItem()
	{
		if(_index < 0) return;
		_dehydrator.RemoveItemFromSlot(_index);
		_index--;
	}

	private void AddItem()
	{
		_index = Math.Min(++_index, _dehydrator.CraftingSlots.Length);
		
		if(_index >= _dehydrator.CraftingSlots.Length) return;
		_dehydrator.InsertItemToSlot(_testItem, _index);
	}

	private void OpenDehydrator()
	{
		EventBus.Instance.BeforeCraftingStationUiOpen(_dehydrator);
	}
}
