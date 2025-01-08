using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Book.Wiki;

/// <summary>
///     Displays an item in the wiki.
/// </summary>
public partial class WikiRelatedItemView : Clickable, ITooltipable
{
	[Export]
	private TextureRect _iconRect;

	private IItemStack _item;
	public void SetItem(IItemStack item)
	{
		_item = item;
		_iconRect.Texture = item?.Icon;
	}

	public string Title => _item?.Name;
	public string Description => _item?.ToolTipDescription;  
	public Control Content => null;
}
