using Godot;
using System;
using GUI.VendingMachine;
using untitledplantgame.Inventory;

public partial class CursorFriend : Control
{
	public static CursorFriend Instance { get; private set; }

	public ItemStack ItemStack
	{
		get => _itemSlot?.ItemStack;
		set => _itemSlot.ItemStack = value;
	}

	[Export] private ItemSlotUI _itemSlot;

	public override void _Ready()
	{
		if (Instance != null)
		{
			QueueFree();
			return;
		}

		Instance = this;
		GetViewport().GuiFocusChanged += OnGuiFocusChanged;
	}

	private void OnGuiFocusChanged(Control node)
	{
		GlobalPosition = 0.5f * (node.GlobalPosition + node.GetGlobalRect().End);
	}


	public override void _Process(double delta)
	{
		if (_itemSlot?.ItemStack != null)
		{
			_itemSlot.Show();
		}
		else
		{
			_itemSlot?.Hide();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
			GlobalPosition = GetGlobalMousePosition();
		}
	}
}
