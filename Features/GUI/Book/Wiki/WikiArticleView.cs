using Godot;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

public partial class WikiArticleView : Node
{
	[Export] private TextureRect _iconTextureRect;
	[Export] private Label _itemNameAndCategory;
	[Export] private Label _itemDescription;
	[Export] private Label _itemStats;
	// TODO show related items
	
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
		_itemNameAndCategory.Text = $"{_itemStack.Name} - {_itemStack.Category.Name}";
		_itemDescription.Text = _itemStack.Description;
		_itemStats.Text = "Stats!... \nStats!... \nStats!...";
	}
}
