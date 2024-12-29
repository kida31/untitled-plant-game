using Godot;
using System;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;

public partial class CursorHandView : Control
{
	[Export]
	private InventoryItemView _itemView;
	CursorInventory _cursorInventory => CursorInventory.Instance;

	public override void _Ready()
	{
		GetViewport().GuiFocusChanged += OnGuiFocusChanged;
		_cursorInventory.ContentChanged += OnContentChanged;
	}

	private void OnContentChanged()
	{
		_itemView.UpdateItemView(_cursorInventory.Content);
	}

	public override void _Process(double delta)
	{
		if (_cursorInventory.Content != null) {
			Show();
		} else {
			Hide();
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion) {
			GlobalPosition = mouseMotion.Position;
		}
	}

	private void OnGuiFocusChanged(Control node)
	{
		var center = node.GetGlobalRect().GetCenter();
		GlobalPosition = center;
	}
}
