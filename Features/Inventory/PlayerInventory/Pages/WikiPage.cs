using Godot;
using System;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

public partial class WikiPage : HBoxContainer
{
	[Export] private WikiItemList _wikiItemList;
	[Export] private WikiArticleView _wikiArticle;
	
	public override void _Ready()
	{
		_wikiItemList.ItemStackPressed += OnWikiItemListOnItemStackPressed;
		GD.Print("Wiki page ready!");
	}
	private void OnWikiItemListOnItemStackPressed(ItemStack stack)
	{
		GD.Print("Item stack pressed!");
		_wikiArticle.SetItemStack(stack);
	}
}
