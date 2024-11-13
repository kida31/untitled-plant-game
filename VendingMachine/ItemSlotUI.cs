using System;
using Godot;
using untitledplantgame.Inventory.Alt;

namespace GUI.VendingMachine;

public partial class ItemSlotUI : Control
{
	public event Action Pressed;
	
	private ItemStack _itemStack;
	[Export] private TextureRect _itemTexture;
	[Export] private Texture2D _placeholderIcon;

	[Export] private Label _quantityLabel;
	[Export] private CanvasItem _highlight;

	public ItemStack ItemStack
	{
		get => _itemStack;
		set => SetItemStack(value);
	}

	public override void _Ready()
	{
		SetItemStack(null);
		FocusEntered += () =>
		{
			_highlight.Show();
			GD.Print($"[{Name}] Entered");
		};

		FocusExited += () =>
		{
			_highlight.Hide();
			GD.Print($"[{Name}] Exited");
		};

		GuiInput += OnGuiInput;
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			Pressed?.Invoke();
			GD.Print($"Pressed {Name}");
		}
	}

	private void SetItemStack(ItemStack itemStack)
	{
		_itemStack = itemStack;
		if (_itemStack == null)
		{
			_itemTexture.Texture = _placeholderIcon;
			_quantityLabel.Text = "";
		}
		else
		{
			_itemTexture.Texture = _itemStack.Icon;
			_quantityLabel.Text = _itemStack.Amount.ToString();
		}
	}
	
	
}
