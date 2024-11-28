using System;
using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class CraftingSlot : Node
{
	public event Action<CraftingSlot> OnCraftingComplete;
	public ItemStack ItemStack { get; set; }
	public bool IsCraftingComplete { get; private set; }
	public int Index { get; set; }
	private Timer _timer;
	private bool _isCrafting;

	public CraftingSlot(ItemStack item, int index)
	{
		ItemStack = item;
		Index = index;
		
		_timer = new Timer();
		AddChild(_timer);
		_timer.Autostart = false;
		_timer.OneShot = true;
		_timer.Timeout += CompleteCrafting;
	}

	public void StartCrafting()
	{
		_isCrafting = true;
		_timer.Start();
	}

	public void RemoveItem()
	{
		QueueFree();
	}

	private void CompleteCrafting()
	{
		_isCrafting = false;
		IsCraftingComplete = true;
		OnCraftingComplete?.Invoke(this);
	}
}
