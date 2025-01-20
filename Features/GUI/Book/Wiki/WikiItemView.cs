using System;
using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Book.Wiki;

/// <summary>
///     This node is a control that displays a single item in the wiki.
/// </summary>
public partial class WikiItemView : Control
{
	/// <summary>
	///     All focus will be redirected to this element instead
	/// </summary>
	private Control _focusAble;

	[Export] private TextureRect _iconTextureRect;

	[Export] private Label _itemName;
	private IItemStack _itemStack;

	[Export] private Texture2D _temporary;

	public IItemStack ItemStack
	{
		get => _itemStack;
		set
		{
			_itemStack = value;
			OnSetItemStack(value);
		}
	}

	// TODO: Make item view focusable instead of using button. Easier to track focus
	public event Action Pressed;

	public override void _Ready()
	{
		// Delegate some object as clickable for focus (selector indicator)
		_focusAble = _iconTextureRect;
		_focusAble.MouseFilter = MouseFilterEnum.Pass;
		_focusAble.FocusMode = FocusModeEnum.All;
		_focusAble.GuiInput += OnGuiInput;

		// Redirect focus to control
		FocusMode = FocusModeEnum.All;
		MouseFilter = MouseFilterEnum.Pass;
		FocusEntered += _focusAble.GrabFocus;
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton button)
		{
			if (button.ButtonIndex == MouseButton.Left && button.Pressed)
			{
				Pressed?.Invoke();
			}
		}

		if (@event.IsAction("ui_accept"))
		{
			Pressed?.Invoke();
		}
	}

	private void OnSetItemStack(IItemStack itemStack)
	{
		_itemName.Text = itemStack.Name;
		_iconTextureRect.Texture = itemStack.Icon ?? _temporary;
	}
}
