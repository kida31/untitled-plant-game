using System;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Statistics.StatTypes;
using untitledplantgame.Systems;

namespace untitledplantgame.Vending;

/// <summary>
///     A vending machine that sells items from its inventory.
///     Encapsulates the selling logic and keeps track of the inventory and gold.
/// </summary>
public class VendingMachine
{
	// Events
	public event Action<IInventory> ContentChanged;
	public event Action<float> PriceMultChanged;
	public event Action<float> FaithMultChanged;

	// Magic Numbers
	private const int MaxSalesCount = 100; // Maximum sales per day
	private const float SalesPercentPerInterval = 0.1f; // Sell 10% of stock per interval
	private const int MinSalesPerInterval = 1; // Sell at least one item per interval
	private const int MinutesPerInterval = 60; // Sell once every hour

	// Properties
	public Inventory.Inventory Inventory => _inventory;
	public float PriceMultiplier => _priceMultiplier;
	public float FaithMultiplier => _faithMultiplier;
	public int Gold => _gold;
	/// <summary>
	///		Whether the vending machine is currently selling items.
	/// </summary>
	public bool IsTicking { get; set; } = true;

	// Private
	private readonly Inventory.Inventory _inventory;
	private int _gold = 0;
	private float _priceMultiplier = 1.0f;
	private float _faithMultiplier = 1.0f;
	private int _salesRemaining = MaxSalesCount;
	private Logger _logger = new("VendingMachine");
	private int _minuteCounter = 0;

	public VendingMachine()
	{
		_inventory = new(12, "Vending Machine");
		_inventory.InventoryChanged += () => ContentChanged?.Invoke(_inventory);

		TimeController.Instance.MinuteTicked += OnMinuteTicked;
		TimeController.Instance.DayChanged += OnEndOfDay;
	}


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
		var itemStacks = _inventory.GetItems();
		if (itemStacks.Count == 0)
		{
			return;
		}

		// Sales count for this transaction is a percent of current supply (rounded up), but at least minimum.
		var totalStock = itemStacks.Sum(it => it?.Amount ?? 0);
		var totalSellCount = (int) Math.Max(Math.Ceiling(SalesPercentPerInterval * totalStock), MinSalesPerInterval);

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
			Assert.AssertTrue(quantity > 0, " mpty item stacks should not be in container");

			// Do not sell more than supply
			var itemSellCount = Math.Min(totalSellCount, quantity);
			_logger.Debug($"{stack.Name}: {totalSellCount} vs. {quantity} => {itemSellCount}");

			// Prices after multiplier are rounded up.
			var goldEarned = Math.Max(1, (int) Math.Ceiling(stack.BaseValue * _priceMultiplier));
			_gold += (int) goldEarned * itemSellCount;

			// Actual sell count has to be deducted from remaining sales
			_salesRemaining -= itemSellCount;
			totalSellCount -= itemSellCount;
			_logger.Debug($"itemsellcount={totalSellCount}");

			// Sold items are no longer in container
			var soldItem = stack.Clone() as ItemStack;
			soldItem!.Amount = itemSellCount;
			_inventory.RemoveItem(soldItem);

			_logger.Info($"Sold {stack.Name} x{itemSellCount} for {goldEarned}g");
			ContentChanged?.Invoke(_inventory);
		}
	}

	public void SetPriceSlider(float f)
	{
		_priceMultiplier = f;
		_faithMultiplier = (float) 1.0 / f;

		PriceMultChanged?.Invoke(_priceMultiplier);
		FaithMultChanged?.Invoke(_faithMultiplier);
	}

	public int CalculateItemPrice(IItemStack item)
	{
		var baseValue = item?.BaseValue ?? 0;
		return (int) Math.Ceiling(baseValue * _priceMultiplier);
	}

	public int WithdrawGold()
	{
		var deducedGold = _gold;
		_gold = 0;

		// Alternatively: Invoke some WithdrawCurrency event in EventBus
		CurrencyFaithOfficer.TheOneAndOnly.ChangeAny(new Currency(), deducedGold);
		ContentChanged?.Invoke(_inventory);
		return deducedGold;
	}

	public void OnEndOfDay(int _)
	{
		_salesRemaining = MaxSalesCount;
	}

	private void OnMinuteTicked(int day, int hour, int minute)
	{
		if (!IsTicking) return;
		
		_minuteCounter += 1;
		if (_minuteCounter >= MinutesPerInterval)
		{
			_minuteCounter = 0;
			SellRandomItems();
		}
	}
}
