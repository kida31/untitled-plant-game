using Godot;
using untitledplantgame.GUI.Items;

namespace untitledplantgame.GUI.Vending;

/// <summary>
///		Displays a vending item with a price.
/// </summary>
public partial class VendingItemView : StorageItemView
{
	[Export] private RichTextLabel _priceLabel;

	public string Price
	{
		get => _price;
		set => _priceLabel.Text = value;
	}

	private string _price;
}
