using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Database;
using untitledplantgame.Inventory;
using static untitledplantgame.Common.Inputs.GameActions.Book;

namespace untitledplantgame.GUI.Book.Wiki;

/// <summary>
///     This node is a control that displays a single article in the wiki.
/// </summary>
public partial class WikiArticleView : Control
{
	public Action ItemChanged;
	
	[Export] private TextureRect _iconTextureRect;
	[Export] private ScrollTextBox _itemDescription;
	[Export] private Label _itemNameAndCategory;

	// TODO show related items

	private IItemStack _itemStack;
	[Export] private Control _itemStats;
	[Export] private WikiRelatedItemView[] _relatedItemViews = Array.Empty<WikiRelatedItemView>();
	public event Action<IItemStack> RelatedItemClicked;

	public override void _Ready()
	{
		for (var i = 0; i < _relatedItemViews.Length; i++)
		{
			var clickable = _relatedItemViews[i];
			var capturedIndex = i;

			void OnPressHandler()
			{
				if (_itemStack is not ItemStack item || item.RelatedItemIds.Count <= capturedIndex)
				{
					return;
				}

				var itemId = item.RelatedItemIds[capturedIndex];
				if (itemId == null)
				{
					return;
				}

				RelatedItemClicked?.Invoke(ItemDatabase.Instance.CreateItemStack(itemId));
			}

			clickable.Pressed += OnPressHandler;
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!IsVisibleInTree()) return;

		if (!@event.IsAction(Common.Inputs.GameActions.Book.West))	return;

		if (@event.IsReleased()) return;

		SwitchPage();
	}

	private void SwitchPage()
	{
		if (_itemStats.Visible)
		{
			_itemStats.Hide();
			_itemDescription.Show();
		}
		else
		{
			_itemStats.Show();
			_itemDescription.Hide();
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
		_itemNameAndCategory.Text = _itemStack != null ? $"{_itemStack.Name} - {_itemStack.Category.Name}" : "";
		_itemDescription.Text = _itemStack?.WikiDescription ?? "";
		// _itemStats.Text = "Stats!... \nStats!... \nStats!..."; // TODO:

		List<IItemStack> relatedItems = new();
		if (_itemStack is ItemStack itemStack)
		{
			relatedItems = itemStack.RelatedItemIds.Select(id => ItemDatabase.Instance.CreateItemStack(id) as IItemStack).ToList();
		}

		for (var i = 0; i < _relatedItemViews.Length; i++)
		{
			var view = _relatedItemViews[i];
			view.SetItem(i < relatedItems.Count ? relatedItems[i] : null);
		}
	}
}
