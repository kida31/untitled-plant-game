using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

// Make a string, so you can change up the category and jump to that. 
public partial class WikiItemView : Control
{
	[Export] private Label _itemName;
	[Export] private BaseButton _detailedWikiItemViewButton;
	[Export] private TextureRect _iconTextureRect;
	[Export] private TextureRect _nameTextureRect;

	[Export] private Texture2D _temporary;
	
	public ItemStack ItemStack;

	public override void _Ready()
	{
		_detailedWikiItemViewButton.FocusEntered += SetFocusOnThisView;
		_detailedWikiItemViewButton.Pressed += ShowDetailedWikiItemView;
		
		// Temporary test
		ItemStack = new ItemStack(
			"seeds",
			"Wonder's Seed", 
			_temporary, 
			"Long Description with a lot of text. Like lots and lots and lots and lots... oh, a butterfly!", 
			ItemCategory.Seed,
			1,
			1
			);

		_itemName.Text = ItemStack.Name;
		_iconTextureRect.Texture = ItemStack.Icon;
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
