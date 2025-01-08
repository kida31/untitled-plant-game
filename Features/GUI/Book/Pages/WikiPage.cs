using System;
using System.Collections.Generic;
using Godot;
using untitledplantgame.GUI.Book.Wiki;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Book.Pages;

/// <summary>
///     This class represents a page in the wiki that displays a list of items and an article
///     for the currently selected item.
/// </summary>
public partial class WikiPage : Control
{
	[Export] private WikiArticleView _wikiArticle;

	[Export] private WikiItemList _wikiItemList;
	public event Action<IItemStack> ItemStackPressed;

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
