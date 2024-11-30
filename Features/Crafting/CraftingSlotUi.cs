using Godot;
using untitledplantgame.Inventory;
using untitledplantgame.VendingMachine;

namespace untitledplantgame.Crafting;

public partial class CraftingSlotUi : ItemSlotUI
{
	[Export] private ProgressBar _progressBar;
	CraftingSlot _craftingSlot;
	
	private bool _isCraftingComplete;
	
	public CraftingSlotUi(CraftingSlot craftingSlot)
	{
		_isCraftingComplete = false;
		_craftingSlot = craftingSlot;
	}

	public CraftingSlotUi()
	{
	}

	private void OnCraftingComplete(CraftingSlot obj)
	{
		// TODO: "Crafting COmplete"
		// -> Parent.ModifyItem
		// -> thisHere.OnCraftingCOmplete
		SetItemStack(obj.ItemStack); // TODO: im not sure this is the latest updated item or the old one
		_isCraftingComplete = true;
		_progressBar.Value = 1;
	}

	private void OnTimePassed(CraftingSlot obj)
	{
		_progressBar.Value = 1 - obj.OnTimePassed(obj); //  TODO: This might loop forever
	}

	protected override void SetItemStack(ItemStack itemStack)
	{
		if (_craftingSlot == null) return;
		
		base.SetItemStack(itemStack);
		_craftingSlot.OnCraftingComplete += OnCraftingComplete;
		_craftingSlot.TimePassed += OnTimePassed;
		
		if(_isCraftingComplete)
			ItemTexture.Modulate = new Color("#7c5f47");
	}
}
