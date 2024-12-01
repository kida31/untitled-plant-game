using Godot;
using untitledplantgame.Common;
using untitledplantgame.VendingMachine;

namespace untitledplantgame.Crafting;

public partial class CraftingSlotUi : ItemSlotUI
{
	[Export] private ProgressBar _progressBar;
	[Export] private TextureRect _craftingCompleteTexture;
	public CraftingSlot CraftingSlot { get; private set; }

	private bool _isCraftingComplete;
	private Logger _logger;

	public override void _Ready()
	{
		base._Ready();
		_logger = new Logger(this);
		_isCraftingComplete = false;
		_logger.Debug("I am ready");
	}

	private void OnCraftingCompleted(CraftingSlot obj)
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
		//ItemTexture.Modulate = color;
		_progressBar.Value = progress;
	}

	public void SetCraftingSlot(CraftingSlot slot)
	{
		if(CraftingSlot != null)
		{
			_logger.Debug("Removing old CraftingSlot");
			CraftingSlot.CraftTimeOut -= OnCraftingCompleted;
			CraftingSlot.ProgressChanged -= UpdateProgressBar;
			_progressBar.Value = 0;
			_isCraftingComplete = false;
			_craftingCompleteTexture.Visible = false;
		}
		CraftingSlot = slot;
		_logger.Debug($"Setting CraftingSlot with item {slot?.ItemStack}");
		ItemStack = slot?.ItemStack;
		
		if(slot != null)
		{
			CraftingSlot.CraftTimeOut += OnCraftingCompleted; //TODO Change event connection for when the item finished modifying
			CraftingSlot.ProgressChanged += UpdateProgressBar;
		}
	}
}
