using Godot;
using untitledplantgame.Inventory.Alt;

namespace GUI.VendingMachine;

public partial class ItemSlotUI : Control
{
	private ItemStack _itemStack;
	[Export] private TextureRect _itemTexture;
	[Export] private Texture2D _placeholderIcon;

	[Export] private Label _quantityLabel;

	public ItemStack ItemStack
	{
		get => _itemStack;
		set => SetItemStack(value);
	}

	private void SetItemStack(ItemStack itemStack)
	{
		_itemStack = itemStack;
		if (_itemStack == null)
		{
			_itemTexture.Texture = _placeholderIcon;
			_quantityLabel.Text = "";
		}
		else
		{
			_itemTexture.Texture = _itemStack.Icon;
			_quantityLabel.Text = _itemStack.Amount.ToString();
		}
	}
}
