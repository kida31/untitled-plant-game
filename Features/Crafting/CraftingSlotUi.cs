using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.VendingMachine;

namespace untitledplantgame.Crafting;

public partial class CraftingSlotUi : ItemSlotUI
{
	[Export] private ProgressBar _progressBar;
	[Export] private TextureRect _craftingCompleteTexture;
	public CraftingSlot CraftingSlot { get; set; }

	private bool _isCraftingComplete;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		_isCraftingComplete = false;
	}

	private void OnCraftingComplete(CraftingSlot obj)
	{
		_logger.Debug("Crafting Complete");
		var item = obj.ItemStack;
		_craftingCompleteTexture.Visible = true;
		// TODO: "Crafting Complete"
		// -> item.ModifyItem
		// -> thisHere.OnCraftingComplete
		SetItemStack(item);
		_isCraftingComplete = true;
		_progressBar.Value = 1;
	}

	private void UpdateProgressBar(double progress)
	{
		var color = new Color(131,90,51, (int)progress);
		ItemTexture.Modulate = color;
		_progressBar.Value = progress;
	}

	public void SetCraftingSlot(CraftingSlot slot)
	{
		if(CraftingSlot != null)
		{
			CraftingSlot.OnCraftingComplete -= OnCraftingComplete;
			CraftingSlot.ProgressChanged -= UpdateProgressBar;
			_progressBar.Value = 0;
			_isCraftingComplete = false;
			_craftingCompleteTexture.Visible = false;
		}
		
		_logger.Debug($"Setting CraftingSlot: {slot}");
		CraftingSlot = slot;
		CraftingSlot.OnCraftingComplete += OnCraftingComplete;
		CraftingSlot.ProgressChanged += UpdateProgressBar;
	}
}
