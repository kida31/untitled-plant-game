using Godot;
using untitledplantgame.GUI.Components;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Book.Wiki;

/// <summary>
///     This node is a control that displays a single item in the wiki.
/// </summary>
public partial class WikiItemView : Clickable
{
	[Export] private TextureRect _iconTextureRect;

	[Export] private Label _itemName;
	private IItemStack _itemStack;

	[Export] private Texture2D _temporary;

	public IItemStack ItemStack
	{
		get => _itemStack;
		set
		{
			_itemStack = value;
			OnSetItemStack(value);
		}
	}

	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Pass;
		FocusMode = FocusModeEnum.All;
	}

	private void OnSetItemStack(IItemStack itemStack)
	{
		_itemName.Text = itemStack.Name;
		_iconTextureRect.Texture = itemStack.Icon ?? _temporary;
	}
}
