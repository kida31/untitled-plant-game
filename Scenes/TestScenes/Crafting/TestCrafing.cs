using Godot;
using System;
using untitledplantgame.Crafting;
using untitledplantgame.Inventory;

public partial class TestCrafing : Node2D
{
	[Export] private DehydratorUi _dehydratorUi;

	private ItemStack _testItem;

	public override void _Ready()
	{
		_testItem = new ItemStack("item_id", "Dead Plants", GD.Load<Texture2D>("res://Assets/OverworldAssets/Plant/DeadPlant.png"),
			"A dead plant", ItemCategory.Plant, 1, 1);
		_dehydratorUi = GetNode<DehydratorUi>("CanvasLayer/DehydratorUi");
	}
}
