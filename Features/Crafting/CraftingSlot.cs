using System;
using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class CraftingSlot
{
	public event Action<CraftingSlot> OnCraftingComplete;
	public event Action<CraftingSlot> TimePassed;
	public ItemStack ItemStack { get; set; }
	public bool IsCraftingComplete { get; private set; }
	public int Index { get; set; }
	private Timer _timer;
	private double _currentTime;
	private double _totalCraftTime;
	private bool isCrafting => _totalCraftTime <= _currentTime;

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
		_timer.Start();
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
