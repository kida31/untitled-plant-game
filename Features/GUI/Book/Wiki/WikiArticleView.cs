using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common.ExtensionMethods;
using untitledplantgame.Inventory;
using untitledplantgame.Item;

namespace untitledplantgame.GUI.Book.Wiki;

/// <summary>
///     This node is a control that displays a single article in the wiki.
/// </summary>
public partial class WikiArticleView : Control
{
	[Export] private TextureRect _iconTextureRect;
	[Export] private RichTextLabel _itemDescription; // RichTextLabel or Label, anything that has .Text
	[Export] private Label _itemNameAndCategory;
	[Export] private RichTextLabel _itemStats;
	[Export] private WikiRelatedItemView[] _relatedItemViews = Array.Empty<WikiRelatedItemView>();
	public event Action<IItemStack> RelatedItemClicked;

	private IItemStack _itemStack;
	private bool _isShowingDescription;
	private Tween _tween;

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

		VisibilityChanged += () =>
		{
			if (!IsVisibleInTree()) return;
			_isShowingDescription = true;
			_itemStats.FadeOut(0f);
			_itemDescription.FadeOut(0);
			_itemDescription.FadeIn(0.4f);
		};
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!IsVisibleInTree() || !@event.IsActionPressed(Common.Inputs.GameActions.Book.West))
		{
			return;
		}

		const float duration = 0.1f;
		_tween?.Stop();
		if (_isShowingDescription)
		{
			// Chain animation
			_tween = _itemDescription.FadeOut(duration);
			_itemStats.Show();
			_tween = _itemStats.FadeIn(duration, tween: _tween);
			ToSignal(_tween, Tween.SignalName.Finished)
				.OnCompleted(_itemDescription.Hide);
		}
		else
		{
			_tween = _itemStats.FadeOut(duration);
			_itemDescription.Show();
			_tween = _itemDescription.FadeIn(duration, tween: _tween);
			ToSignal(_tween, Tween.SignalName.Finished)
				.OnCompleted(_itemStats.Hide);
		}

		_isShowingDescription = !_isShowingDescription;
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
		_itemStats.Text = "Stats!... \nStats!... \nStats!..."; // TODO:

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
