using Godot;
using untitledplantgame.Inventory;
using untitledplantgame.VendingMachine;

namespace untitledplantgame.Crafting;

public partial class CraftingSlotUi : ItemSlotUI
{
	[Export] private ProgressBar _progressBar;
	public CraftingSlot CraftingSlot { get; set; }

	private bool _isCraftingComplete;

	public CraftingSlotUi(CraftingSlot craftingSlot)
	{
		_isCraftingComplete = false;
		CraftingSlot = craftingSlot;
	}

	public CraftingSlotUi()
	{
	}

	private void OnCraftingComplete(CraftingSlot obj)
	{
		var item = obj.ItemStack;
		// TODO: "Crafting Complete"
		// -> item.ModifyItem
		// -> thisHere.OnCraftingComplete
		SetItemStack(item);
		_isCraftingComplete = true;
		_progressBar.Value = 1;
	}

	private void UpdateProgressBar(double progress)
	{
		_progressBar.Value = progress;
	}

	protected override void SetItemStack(ItemStack itemStack)
	{
		if (CraftingSlot == null) return;

		base.SetItemStack(itemStack);
		CraftingSlot.OnCraftingComplete += OnCraftingComplete;
		CraftingSlot.ProgressChanged += UpdateProgressBar;

		if (_isCraftingComplete)
			ItemTexture.Modulate = new Color("#7c5f47");
	}
}
