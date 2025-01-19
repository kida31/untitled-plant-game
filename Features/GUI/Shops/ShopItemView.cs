using Godot;
using untitledplantgame.Common;
using untitledplantgame.GUI.Items;

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
		if (ItemStack == null) return null;
		
		var label = new RichTextLabel();
		label.BbcodeEnabled = true;
		label.Text = ItemStack == null ? "" : $"[center]{ItemStack.BaseValue}{BbImage.Coin}";
		label.AutowrapMode = TextServer.AutowrapMode.Off;
		label.FitContent = true;
		label.SizeFlagsHorizontal = SizeFlags.Expand;
		label.SizeFlagsVertical = SizeFlags.Expand;
		return label;
	}
}
