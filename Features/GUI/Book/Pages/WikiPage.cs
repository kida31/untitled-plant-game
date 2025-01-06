using Godot;
using System;
using System.Collections.Generic;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

public partial class WikiPage : Control
{
	public event Action<IItemStack> ItemStackPressed;

	[Export] private WikiItemList _wikiItemList;
	[Export] private WikiArticleView _wikiArticle;

	public override void _Ready()
	{
		_wikiItemList.ItemStackPressed += i => ItemStackPressed?.Invoke(i);
		_wikiArticle.RelatedItemClicked += OnRelatedItemClicked;
		
		VisibilityChanged += OnVisibilityChanged;
	}

	private void OnVisibilityChanged()
	{
		if (IsVisibleInTree())
		{
			_wikiItemList.GrabFocus();
		}
	}

	public void UpdateItems(List<ItemStack> items)
	{
		_wikiItemList.SetItems(items);
	}

	public void UpdateArticle(IItemStack content)
	{
		_wikiArticle.UpdateItemStack(content);
	}

	private void OnRelatedItemClicked(IItemStack stack)
	{
		_wikiItemList.ScrollTo(stack);
	}

}
