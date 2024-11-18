using Godot;

namespace untitledplantgame.Inventory.GUI;

public partial class ItemTooltip : Control
{
	[Export] private Label _nameLabel;
	[Export] private Label _descriptionLabel;

	public ItemStack ItemStack {
		get => _itemStack;
		set
		{
			_itemStack = value;
			UpdateContent();
		}
	}

	private ItemStack _itemStack;

	private void UpdateContent()
	{
		_nameLabel.Text = _itemStack.Name;
		_descriptionLabel.Text = _itemStack.Description;
	}
}
