using Godot;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

public partial class WikiArticleView : Node
{
	[Export] private TextureRect _iconTextureRect;
	[Export] private Label _itemNameAndCategory;
	[Export] private Label _itemDescription;
	[Export] private Label _itemStats;
	[Export] private TextureRect _bottomTextureRect;
	
	private ItemStack _itemStack;
	
	public void UpdateItemStack(ItemStack itemStack)
	{
		_itemStack = itemStack;
		// TODO: fetch wiki data here or in controller
		UpdateView();
	}

	private void UpdateView()
	{
		_iconTextureRect.Texture = _itemStack.Icon;
		_itemNameAndCategory.Text = _itemStack.Name + ", of Type: " + _itemStack.Category.Name;
		_itemDescription.Text = _itemStack.Description;
		_itemStats.Text = "Stats!... \nStats!... \nStats!...";
		_bottomTextureRect.Texture = null; // Also not sure where to pull these from. 
	}
}
