using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class CraftingSlot
{
	// We cooked.
	public event Action<CraftingSlot> CraftTimeOut;
	public event Action<double> ProgressChanged;
	public IItemStack ItemStack { get; set; }
	public bool IsCraftingComplete;
	private bool _isCrafting; 
	private double _currentTime;
	private double _totalCraftTime;
	private Logger _logger;

	public CraftingSlot()
	{
		_logger = new Logger("CraftingSlot");
	}

	public CraftingSlot(IItemStack item)
	{
		_isCrafting = false;
		ItemStack = item;
		_logger = new Logger("CraftingSlot");
	}

	public void Process(double delta)
	{
		if (IsCraftingComplete || !_isCrafting) return;
		_currentTime -= delta;
		
		if (_currentTime <= 0)
		{
			CompleteCrafting();
			_logger.Debug("Crafting complete");
			return;
		}

		var progress = 1 - _currentTime / _totalCraftTime;
		OnTimePassed(progress);
	}

	private void OnTimePassed(double timeProgress)
	{
		ProgressChanged?.Invoke(timeProgress);
	}

	public void AddItemAndStartCrafting(IItemStack itemStack, double craftTime)
	{
		ItemStack = itemStack;
		_totalCraftTime = craftTime;
		_currentTime = _totalCraftTime;
		_isCrafting = true;
	}

	public void RemoveItem()
	{
		ItemStack = null;
		_isCrafting = false;
	}

	private void CompleteCrafting()
	{
		IsCraftingComplete = true;
		_isCrafting = false;
		CraftTimeOut?.Invoke(this);
	}
}
