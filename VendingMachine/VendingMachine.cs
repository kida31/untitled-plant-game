using System;
using System.Diagnostics;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.VendingMachine;

public class VendingMachine
{
	// Events
	public event Action<IInventory> ContentChanged;
	public event Action<float> PriceMultChanged;
	public event Action<float> FaithMultChanged;

	// Magic Numbers
	private const int MaxSalesCount = 100;
	private const float SalesPercentPerInterval = 0.1f;

	// Properties
	public Inventory.Inventory Inventory => _inventory;
	public float PriceMultiplier => _priceMultiplier;
	public float FaithMultiplier => _faithMultiplier;
	public int Gold => _gold;

	// Private
	private readonly Inventory.Inventory _inventory = new(12, "Vending Machine");
	private int _gold = 0;
	private float _priceMultiplier = 1.0f;
	private float _faithMultiplier = 1.0f;
	private int _salesRemaining = MaxSalesCount;
	private Logger _logger = new("VendingMachine");

	/// <summary>
	/// Sells a random item from the vending machine's inventory.
	/// </summary>
	/// <remarks>
	/// This method performs the following steps:
	/// <list type="number">
	/// <item><description>Checks if any sales are remaining. If not, the method returns immediately.</description></item>
	/// <item><description>Checks if there are any items in supply. If not, the method returns immediately.</description></item>
	/// <item><description>Calculates the total number of items to sell based on a percentage of the current supply, but ensures at least one item is sold.</description></item>
	/// <item><description>Sorts the items by price in descending order to sell the most expensive items first.</description></item>
	/// <item><description>Iterates through the sorted items and sells them until the total sell count reaches zero or the inventory is depleted.</description></item>
	/// <item><description>Updates the total gold earned by multiplying the item price by a price multiplier and rounding up.</description></item>
	/// <item><description>Deducts the actual sell count from the remaining sales and removes the sold items from the inventory.</description></item>
	/// </list>
	/// </remarks>
	public void SellRandomItems()
	{
		// Check if any sales remaining
		if (_salesRemaining <= 0)
		{
			return;
		}

		// Check if any items in supply
		var itemStacks = _inventory.GetContents();
		if (itemStacks.Count == 0)
		{
			return;
		}

		// Sales count for this transaction is a percent of current supply, but at least one.
		var totalSellCount = (int)Math.Ceiling(Math.Max(SalesPercentPerInterval * itemStacks.Count, 1));

		// Sort by price descending, sell most expensive first.
		var itemsByPrice = _inventory.OrderByDescending(stack => stack?.BaseValue ?? 0).ToList();

		foreach (var stack in itemsByPrice)
		{
			// Can stop selling once count has reached zero
			if (totalSellCount == 0)
			{
				break;
			}

			if (stack == null)
			{
				continue;
			}

			var quantity = stack.Amount;
			Debug.Assert(quantity > 0); // Empty item stacks should not be in container

			// Do not sell more than supply
			var itemSellCount = Math.Min(totalSellCount, quantity);
			_logger.Debug($"{stack.Name}: {totalSellCount} vs. {quantity} => {itemSellCount}");

			// Prices after multiplier are rounded up.
			var goldEarned = Math.Max(1, (int)Math.Ceiling(stack.BaseValue * _priceMultiplier));
			_gold += (int)goldEarned * itemSellCount;

			// Actual sell count has to be deducted from remaining sales
			_salesRemaining -= itemSellCount;
			totalSellCount -= itemSellCount;
			_logger.Debug($"itemsellcount={totalSellCount}");

			// Sold items are no longer in container
			_inventory.RemoveItem(
				new ItemStack(stack.Id, stack.Name, stack.Icon, stack.Description, stack.Category, stack.MaxStackSize, stack.BaseValue)
				{
					Amount = itemSellCount,
				}
			);

			_logger.Info($"Sold {stack.Name} x{itemSellCount} for {goldEarned}g");
			ContentChanged?.Invoke(_inventory);
		}
	}

	public void SetPriceSlider(float f)
	{
		_priceMultiplier = f;
		_faithMultiplier = (float)1.0 / f;

		PriceMultChanged?.Invoke(_priceMultiplier);
		FaithMultChanged?.Invoke(_faithMultiplier);
	}

	public int WithdrawGold()
	{
		var deducedGold = _gold;
		_gold = 0;
		return deducedGold;
	}

	public void OnEndOfDay()
	{
		_salesRemaining = MaxSalesCount;
	}
}
