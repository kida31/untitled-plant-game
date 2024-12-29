using System;
using System.Collections.Generic;
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
	[Export] private WikiRelatedItemView[] _relatedItemViews = new WikiRelatedItemView[0];

	// TODO show related items
	
	private IItemStack _itemStack;

	public override void _Ready()
	{
		for(var i = 0; i < _relatedItemViews.Length; i++){
			var clickable = _relatedItemViews[i];
			var capturedIndex = i;
			void OnPressHandler() {
				if (_itemStack is not ItemStack item || item.RelatedItemIds.Count <= capturedIndex) {
					return;
				}

				var itemId = item.RelatedItemIds[capturedIndex];
				if (itemId == null) {
					return;
				}

				RelatedItemClicked?.Invoke(ItemDatabase.Instance.CreateItemStack(itemId));
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

		List<IItemStack> relatedItems = new();		
		if (_itemStack is ItemStack itemStack) {
			relatedItems = itemStack.RelatedItemIds.Select(id => ItemDatabase.Instance.CreateItemStack(id) as IItemStack).ToList();
		}

		for (int i = 0; i < _relatedItemViews.Length; i++)
		{
			var view = _relatedItemViews[i];
			view.SetItem(i < relatedItems.Count ? relatedItems[i] : null);
		}
	}

}
