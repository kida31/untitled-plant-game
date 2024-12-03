using Godot;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Tabs;

public partial class DetailedItemView : Control
{
	[Export] private TextureRect _itemTextureRect;
	[Export] private Label _itemDescription;

	public void SetItemTextureRect(Texture2D picture)
	{
		_itemTextureRect.Texture = picture;
	}
	
	public void SetItemDescription(string description)
	{
		_itemDescription.Text = description;
	}
}
