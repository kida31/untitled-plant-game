using System;
using Godot;
namespace untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

// Make a string, so you can change up the category and jump to that. 
public partial class WikiItemView : Control
{
	public event Action Pressed;

	[Export] private Label _itemName;
	[Export] private BaseButton _detailedWikiItemViewButton;
	[Export] private TextureRect _iconTextureRect;
	[Export] private TextureRect _nameTextureRect;

	[Export] private Texture2D _temporary;

	private ItemStack _itemStack;
	public ItemStack ItemStack
	{
		get => _itemStack;
		set
		{
			_itemStack = value;
			OnSetItemStack(value);
		}
	}

	public override void _Ready()
	{
		_detailedWikiItemViewButton.FocusEntered += SetFocusOnThisView;
		_detailedWikiItemViewButton.Pressed += () => Pressed?.Invoke();

	}
	
	private void OnSetItemStack(ItemStack itemStack)
	{
		
		_itemName.Text = itemStack.Name;
		_iconTextureRect.Texture = itemStack.Icon ?? _temporary;
	}

	public override void _GuiInput(InputEvent @event)
	{
		// We use the button for this, until we have custom highlight for pressed/hovered/selected
		// Use FocusMode = ALL for this to work
		return;
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

	public void GrabFocusToButton()
	{
		_detailedWikiItemViewButton.GrabFocus();
	}
	
	private void SetFocusOnThisView()
	{
		EventBus.Instance.ScrollContainerViewChanged(this);
	}
}
