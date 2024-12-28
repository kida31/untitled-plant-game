using System;
using System.Linq;
using Godot;
using untitledplantgame.Database;

namespace untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

public partial class WikiArticleView : Node
{
	public event Action<IItemStack> RelatedItemClicked;

	[Export] private TextureRect _iconTextureRect;
	[Export] private Label _itemNameAndCategory;
	[Export] private Label _itemDescription;
	[Export] private Label _itemStats;
	/// <summary>
	/// These control objects need to have obj.Texture
	/// </summary>
	[Export] private Clickable[] _relatedItemClickables = new Clickable[0];

	// TODO show related items
	
	private IItemStack _itemStack;

	public override void _Ready()
	{
		for(var i = 0; i < _relatedItemClickables.Length; i++){
			var clickable = _relatedItemClickables[i];
			var capturedIndex = i;
			void OnPressHandler() {
				var itemId = (_itemStack as ItemStack)?.RelatedItemIds[capturedIndex];
				RelatedItemClicked?.Invoke(itemId != null ? ItemDatabase.Instance.CreateItemStack(itemId) : null);
			}

			clickable.Pressed += OnPressHandler;
		}
	}

	public void UpdateItemStack(IItemStack itemStack)
	{
		_itemStack = itemStack;
		// TODO: fetch wiki data here or in controller
		UpdateView();
	}

	private void UpdateView()
	{
		_iconTextureRect.Texture = _itemStack?.Icon ?? null;
		_itemNameAndCategory.Text = _itemStack != null ? $"{_itemStack.Name} - {_itemStack.Category.Name}": "";
		_itemDescription.Text = _itemStack?.Description ?? "";
		_itemStats.Text = "Stats!... \nStats!... \nStats!...";

		var relatedItems = (_itemStack as ItemStack).RelatedItemIds
			.Select(id => ItemDatabase.Instance.CreateItemStack(id)).ToList();

		for (int i = 0; i < _relatedItemClickables.Length; i++)
		{
			// TODO: Type-safe this
			var textRect = _relatedItemClickables[i].GetChild<TextureRect>(0);
			
			var item = i < relatedItems.Count ? relatedItems[i] : null;
			textRect.Texture = item?.Icon ?? null;
		}
	}

}
