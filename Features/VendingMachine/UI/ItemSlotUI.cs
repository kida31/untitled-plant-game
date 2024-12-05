using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.VendingMachine;

public partial class ItemSlotUI : Control, IItemSlotUI
{
	public event Action Pressed;

	[Export]
	private TextureRect _itemTexture;

	[Export]
	private Texture2D _placeholderIcon;

	[Export]
	private Label _quantityLabel;

	[Export]
	private CanvasItem _highlight;

	public IItemStack ItemStack
	{
		get => _itemStack;
		set => SetItemStack(value);
	}

	private IItemStack _itemStack;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		SetItemStack(null);
		FocusEntered += () =>
		{
			_highlight.Show();
			_logger.Debug($"[{Name}] Entered");
		};

		FocusExited += () =>
		{
			_highlight.Hide();
			_logger.Debug($"[{Name}] Exited");
		};

		GuiInput += OnGuiInput;
	}

	protected virtual void SetItemStack(IItemStack itemStack)
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

	private void OnGuiInput(InputEvent @event)
	{
		// || (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Left && mb.Pressed)
		if (@event.IsActionPressed("ui_accept"))
		{
			Pressed?.Invoke();
			_logger.Debug($"Pressed {Name}");
		}
	}
}
