using System;
using Godot;
using untitledplantgame.Inventory;


namespace untitledplantgame.Crafting;

public partial class CraftingSlot : Node
{
	public event Action<ItemStack> OnCraftingComplete;
	public ItemStack ItemStack { get; set; }
	public bool IsCraftingComplete => !_isCrafting;
	private Timer _timer;
	private bool _isCrafting;
	
	public override void _Ready()
	{
		_timer = new Timer();
		AddChild(_timer);
		_timer.Autostart = false;
		_timer.OneShot = true;
		_timer.Timeout += CraftingComplete;
	}
	
	public void StartCrafting()
	{
		_isCrafting = true;
		_timer.Start();
	}
	
	public void RemoveItem()
	{
		ItemStack = null;
		_isCrafting = false;
		_timer.Stop();
	}

	private void CraftingComplete()
	{
		_isCrafting = false;
		
		OnCraftingComplete?.Invoke(ItemStack);
	}
}
