using System;
using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class CraftingSlot
{
	// We cooked.
	public event Action<CraftingSlot> OnCraftingComplete;
	public event Action<double> ProgressChanged;
	public ItemStack ItemStack { get; set; }
	public bool IsCraftingComplete { get; private set; } // We have cooked. Still good to know
	private Timer _timer;
	private double _totalCraftTime;

	public CraftingSlot()
	{
	}

	public CraftingSlot(ItemStack item)
	{
		ItemStack = item;
	}

	public void Process(double delta)
	{
		if (IsCraftingComplete || _timer == null) return;

		if (_timer.TimeLeft <= 0)
		{
			CompleteCrafting();
			return;
		}

		var progress = 1 - _timer.TimeLeft / _totalCraftTime;
		OnTimePassed(progress);
	}

	public void OnTimePassed(double timeProgress)
	{
		ProgressChanged?.Invoke(timeProgress);
	}

	public void AddItemAndStartCrafting(ItemStack itemStack, double craftTime)
	{
		ItemStack = itemStack;
		_totalCraftTime = craftTime;
		StartCraftProcess();
	}

	public void RemoveItem()
	{
		ItemStack = null;
		_timer.Stop();
		IsCraftingComplete = false;
		_timer.WaitTime = 0;
	}

	private void CompleteCrafting()
	{
		IsCraftingComplete = true;
		OnCraftingComplete?.Invoke(this);
	}

	private void StartCraftProcess()
	{
		_timer = new Timer();
		_timer.Autostart = true;
		_timer.OneShot = true;
		_timer.Timeout += CompleteCrafting;
	}
}
