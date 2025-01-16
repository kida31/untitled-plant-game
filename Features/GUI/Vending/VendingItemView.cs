using System.Collections.Generic;
using Godot;
using untitledplantgame.GUI.Items;

namespace untitledplantgame.GUI.Vendoring;

public partial class VendingItemView : StorageItemView
{
	[Export] private Label _priceLabel;

	public string Price
	{
		get => _price;
		set => _priceLabel.Text = value;
	}

	private string _price;
}
