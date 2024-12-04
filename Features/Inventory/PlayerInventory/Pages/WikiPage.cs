using Godot;
using System;
using System.Collections.Generic;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

public partial class WikiPage : HBoxContainer
{
	public event Action<ItemStack> ItemStackPressed;

	[Export] private WikiItemList _wikiItemList;
	[Export] private WikiArticleView _wikiArticle;

	public override void _Ready()
	{
		_wikiItemList.ItemStackPressed += i => ItemStackPressed?.Invoke(i);
	}

	public void UpdateItems(List<ItemStack> items)
	{
		_wikiItemList.SetItems(items);
	}

	public void UpdateArticle(ItemStack content)
	{
		_wikiArticle.SetItemStack(content);
	}
}
