using Godot;
using untitledplantgame.GUI.Book.Inventories;

namespace untitledplantgame.GUI.VendingMachine;

public partial class VendingItemView : InventoryItemView
{
	[Export] private Label _priceLabel;

	public int Price
	{
		get => _price;
		set
		{
			_price = value;
			_priceLabel.Text = $"{_price}g";
		}
	}

	private int _price;
}
