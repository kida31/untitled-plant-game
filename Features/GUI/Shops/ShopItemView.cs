using Godot;
using untitledplantgame.Common;
using untitledplantgame.GUI.Items;
using untitledplantgame.Inventory;

namespace untitledplantgame.GUI.Shops;

public partial class ShopItemView : NewInventoryItemView
{
	[Export] private RichTextLabel _priceLabel;
	public override Control Content => StatsToControl();

	protected override void UpdateContent()
	{
		base.UpdateContent();
		_priceLabel.Text = ItemStack == null ? "" : $"[center]{ItemStack.BaseValue}{BbImage.Coin}";
	}

	private Control StatsToControl()
	{
		var label = new RichTextLabel();
		label.BbcodeEnabled = true;
		label.Text = ItemStack == null ? "" : $"[center]{ItemStack.BaseValue}{BbImage.Coin}";
		label.FitContent = true;
		return label;
	}
}
