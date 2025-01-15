using Godot;
using untitledplantgame.GUI.Items;

namespace untitledplantgame.GUI.Vendoring;

public partial class VendingItemView : StorageItemView
{
	[Export] private Label _priceLabel;

	public int Price
	{
		get => _price;
		set
		{
			_price = value;
			_priceLabel.Text = _price > 0 ? $"{_price}g" : "";
		}
	}

	private int _price;
}
