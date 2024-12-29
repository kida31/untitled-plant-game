using Godot;
using System;
using untitledplantgame.Inventory;

public partial class WikiRelatedItemView : Clickable
{
	[Export]
	private TextureRect _iconRect;

	public void SetItem(IItemStack item) {
		_iconRect.Texture = item?.Icon ?? null;
	}
}
