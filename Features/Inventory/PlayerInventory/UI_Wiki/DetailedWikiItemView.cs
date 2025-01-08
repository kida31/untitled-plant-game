using Godot;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

public partial class DetailedWikiItemView : Node
{
	[Export] private TextureRect _iconTextureRect;
	[Export] private Label _itemNameAndCategory;
	[Export] private Label _itemDescription;
	[Export] private Label _itemStats;
	[Export] private TextureRect _bottomTextureRect;

	public void SetDetailedWikiItemView(ItemStack itemStack)
	{
		_iconTextureRect.Texture = itemStack.Icon;
		_itemNameAndCategory.Text = itemStack.Name + ", of Type: " + itemStack.Category.Name;
		_itemDescription.Text = itemStack.ToolTipDescription;
		_itemStats.Text = "Stats!... \nStats!... \nStats!...";
		_bottomTextureRect.Texture = null; // Also not sure where to pull these from. 
	}
}
