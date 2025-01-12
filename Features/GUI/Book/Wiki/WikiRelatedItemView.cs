using Godot;
using untitledplantgame.GUI.Components;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Book.Wiki;

/// <summary>
///     Displays an item in the wiki.
/// </summary>
public partial class WikiRelatedItemView : Clickable, ITooltipable
{
	[Export]
	private TextureRect _iconRect;

	
	public bool TooltipEnabled => _item != null;
	public string Title => _item?.Name;
	public string Description => _item?.ToolTipDescription;  
	public Control Content => null;
	
	private IItemStack _item;
	
	public void SetItem(IItemStack item)
	{
		_item = item;
		_iconRect.Texture = item?.Icon;
	}


}
