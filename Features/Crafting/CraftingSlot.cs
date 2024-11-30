using System;
using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class CraftingSlot
{
	// We cooked.
	public event Action<CraftingSlot> OnCraftingComplete;
	public event Action<CraftingSlot> TimePassed; // TODO: "Progress has changed: This is the remaining time left in percent"
	public ItemStack ItemStack { get; set; }
	public bool IsCraftingComplete { get; private set; } // We have cooked. Still good to know
	public int Index { get; set; } // TODO: This is not used. Remove it
	private Timer _timer;
	private double _currentTime; // TODO: We also have a timer.
	private double _totalCraftTime;
	private bool isCrafting => _totalCraftTime <= _currentTime; // TODO: This is not used

	public CraftingSlot()
	{
	}
	
	public CraftingSlot(ItemStack item, int index)
	{
		ItemStack = item;
		Index = index;
	}

	public void Process(double delta)
	{
		if (IsCraftingComplete) return;

		if (isCrafting)
		{
			CompleteCrafting();
			return;
		}

		OnTimePassed(this);
	}

	// TODO: AddAndProcessItem
	public void AddItem(ItemStack itemStack, double craftTime)
	{
		ItemStack = itemStack;
		_totalCraftTime = craftTime;
		_currentTime = 0;
		_timer = new Timer();
		_timer.Autostart = true;
		_timer.OneShot = true;
		_timer.Timeout += CompleteCrafting;
	}

	public void RemoveItem()
	{
		ItemStack = null;
		_timer.Stop();
		IsCraftingComplete = false;
		_currentTime = 0;
		_timer.WaitTime = 0;
		_timer.Start(); // TODO: Why do we start here?
	}

	private void CompleteCrafting()
	{
		IsCraftingComplete = true;
		OnCraftingComplete?.Invoke(this);
	}

	public double OnTimePassed(CraftingSlot obj)
	{
		TimePassed?.Invoke(obj);
		return _timer.TimeLeft / _totalCraftTime;
	}
}
