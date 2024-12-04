using Godot;
using System;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.PlayerInventory.UI_Wiki;

public partial class WikiPage : HBoxContainer
{
	[Export] private WikiItemList _wikiItemList;
	[Export] private WikiArticleView _wikiArticle;

	private Logger _logger;
	
	public override void _Ready()
	{
		_logger = new(this);
		_wikiItemList.ItemStackPressed += OnWikiItemListOnItemStackPressed;
	}
	
	private void OnWikiItemListOnItemStackPressed(ItemStack stack)
	{
		_logger.Debug("Set new wiki article.");
		_wikiArticle.SetItemStack(stack);
	}
}
