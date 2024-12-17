using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

// Make a string, so you can change up the category and jump to that. 
public partial class WikiItemView : Control
{
	// TODO: Make item view focusable instead of using button. Easier to track focus
	public event Action Pressed;

	[Export] private Label _itemName;
	[Export] private BaseButton _detailedWikiItemViewButton;
	[Export] private TextureRect _iconTextureRect;

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

	/// <summary>
	/// All focus will be redirected to this element instead
	/// </summary>
	private Control _focusAble;
	private IItemStack _itemStack;

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
