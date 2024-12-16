using Godot;
using System;
using untitledplantgame.Inventory;
using untitledplantgame.Medicine;

public partial class TestItemDatabase : Node2D
{
	[Export] private ItemStack _item;
	public override void _Ready()
	{
		var json = new Json();
		var stack = new ItemStack("any", "any", null, "some recognizable item", ItemCategory.Medicine, baseValue: 69, maxStackSize: 1, amount: 1);
		var medicine = new MedicineComponent("any", 69);
		stack.AddComponent(medicine);
		ResourceSaver.Save(stack, "res://Scenes/TestScenes/Item/MyAss.tres");
		
		_item = ResourceLoader.Load<ItemStack>("res://Scenes/TestScenes/Item/MyAss.tres");
		
		
	}
}
