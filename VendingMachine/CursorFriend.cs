using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.VendingMachine;

/// <summary>
/// Represents Cursor "storage" for the player when picking up items from some inventory
/// Functional + UI
/// </summary>
public partial class CursorFriend : Control
{
	public static CursorFriend Instance { get; private set; }

	[Export]
	private ItemSlotUI _itemSlot;

	public ItemStack ItemStack
	{
		get => _itemSlot?.ItemStack;
		set => _itemSlot.ItemStack = value;
	}

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

	private void OnGuiFocusChanged(Control node)
	{
		GlobalPosition = 0.5f * (node.GlobalPosition + node.GetGlobalRect().End);
	}
}
