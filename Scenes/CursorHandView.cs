using Godot;
using untitledplantgame.GUI.Book.Inventories;
using untitledplantgame.Inventory;

namespace untitledplantgame.Scenes;

/// <summary>
///     This is the cursor hand view that shows the item that the player is currently holding.
/// </summary>
public partial class CursorHandView : Control
{
	[Export] private InventoryItemView _itemView;

	// Does not match property naming rule, because the singleton is just an example ref placeholder.
	// Future implementation may have own cursor inventories for different purposes.
	private CursorInventory _cursorInventory => CursorInventory.Instance;

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
		if (_cursorInventory.Content != null)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			GlobalPosition = mouseMotion.Position;
		}
	}

	private void OnGuiFocusChanged(Control node)
	{
		var center = node.GetGlobalRect().GetCenter();
		GlobalPosition = center;
	}
}
