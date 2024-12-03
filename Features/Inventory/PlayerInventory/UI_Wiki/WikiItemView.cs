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
		_detailedWikiItemViewButton.Pressed += ShowDetailedWikiItemView;
	}
	
	private void OnSetItemStack(ItemStack itemStack)
	{
		
		_itemName.Text = itemStack.Name;
		_iconTextureRect.Texture = itemStack.Icon ?? _temporary;
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (_detailedWikiItemViewButton.HasFocus())
		{
			if (@event is InputEventJoypadButton button)
			{
				if (button.ButtonIndex == JoyButton.A)
				{
					ShowDetailedWikiItemView();
				}
			}
		}
	}

	public void GrabFocusToButton()
	{
		_detailedWikiItemViewButton.GrabFocus();
	}

	private void ShowDetailedWikiItemView()
	{
		EventBus.Instance.UiWikiItemClicked(ItemStack);
	}

	private void SetFocusOnThisView()
	{
		EventBus.Instance.ScrollContainerViewChanged(this);
	}
}
