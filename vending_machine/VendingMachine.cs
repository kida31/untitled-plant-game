using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class VendingMachine : Node
{
	// Magic Numbers
	private static readonly int MAX_SALES = 100;
	private static readonly float SALES_PERCENT_PER_INTERVAL = 0.1f;

	// State
	private ItemContainer<ISellable> _itemContainer;
	private int _gold = 0;
	private float _priceMultiplier = 1.0f;
	private float _faithMultiplier = 1.0f;
	private int _salesRemaining = MAX_SALES;

	// Properties
	public ItemContainer<ISellable> ItemContainer => _itemContainer;
	public float PriceMultiplier => _priceMultiplier;
	public float FaithMultiplier => _faithMultiplier;

	// GUI and Interaction
	[Export]
	private Button WithdrawButton;
	[Export]
	
	private 
    public override void _Ready()
    {
        
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
    public void SellRandomItem()
	{
		// Check if any sales remaining
		if (_salesRemaining <= 0) {
			return;
		}

		// Check if any items in supply
		var totalItemCount = _itemContainer.Items.Select((keyValue) => keyValue.Value).Sum();
		if (totalItemCount == 0) {
			return;
		}

		// Sales count for this transaction is a percent of current supply, but at least one.
		int totalSellCount = (int) Math.Max(SALES_PERCENT_PER_INTERVAL * totalItemCount, 1);

		// Sort by price descending, sell most expensive first.
		var itemsByPrice = _itemContainer.Items.OrderBy(keyValue => keyValue.Key.Price);

		foreach (var keyValue in itemsByPrice)
		{
			// Can stop selling once count has reached zero
			if (totalSellCount == 0) break;

			var (item, quantity) = keyValue;
			Debug.Assert(quantity > 0); // Empty item stacks should not be in container

			// Do not sell more than supply
			var itemSellCount = Math.Min(totalSellCount, quantity);

			// Prices after multiplier are rounded up.
			_gold += (int) Math.Ceiling(item.Price * _priceMultiplier) * itemSellCount;

			// Actual sell count has to be deducted from remaining sales
			_salesRemaining -= itemSellCount;

			// Sold items are no longer in container
			_itemContainer.RemoveItem(item, itemSellCount);
		}
	}

	public void SetPriceSlider(float f) {

	}

	public int WithdrawGold() {
		var deducedGold = _gold;
		_gold = 0;
		return deducedGold;
	}

	public void OnEndOfDay() {
		_salesRemaining = MAX_SALES;
	}
}
