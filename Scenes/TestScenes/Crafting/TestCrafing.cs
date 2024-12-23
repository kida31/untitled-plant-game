using Godot;
using System;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Crafting;
using untitledplantgame.Inventory;
using untitledplantgame.Item;
using untitledplantgame.Item.Components;
using untitledplantgame.Medicine;
using static untitledplantgame.Medicine.MedicinalEffect;

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
		var medicine = new MedicinalComponent(new System.Collections.Generic.Dictionary<MedicinalEffect, int>
		{
			{Warming, 3},
			{Antibacterial, 2}
		});
		_testItem = new ItemStack(
			id: "TestDummy",
			name: "Dead Plant",
			description: "A plant that died due to your carelessness.",
			icon: texture,
			category: ItemCategory.Plant,
			baseValue: 1,
			maxStackSize: 1,
			amount: 1,
			components: new Array<AComponent>()
			{
				new PlantComponent(),
				medicine
			}
		);
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
