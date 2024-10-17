using System;
using System.Diagnostics;
using System.Linq;
using InventoryV0;

public class VendingMachine
{
    // Magic Numbers
    private const int MAX_SALES = 100;
    private const float SALES_PERCENT_PER_INTERVAL = 0.1f;

    // State
    private ItemStack<ISellable>[] _items;
    private int _gold = 0;
    private float _priceMultiplier = 1.0f;
    private float _faithMultiplier = 1.0f;
    private int _salesRemaining = MAX_SALES;

    // Properties
    public ItemStack<ISellable>[] Items => _items;
    public float PriceMultiplier => _priceMultiplier;
    public float FaithMultiplier => _faithMultiplier;

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
        var totalItemCount = _items.Select((stack) => stack.Quantity).Sum();
        if (totalItemCount == 0)
        {
            return;
        }

        // Sales count for this transaction is a percent of current supply, but at least one.
        var totalSellCount = (int) Math.Max(SALES_PERCENT_PER_INTERVAL * totalItemCount, 1);

        // Sort by price descending, sell most expensive first.
        var itemsByPrice = _items.OrderBy(stack => stack.Item.Price).ToList();

        for (var index = 0; index < itemsByPrice.Count; index++)
        {
            var stack = itemsByPrice[index];

            // Can stop selling once count has reached zero
            if (totalSellCount == 0) break;

            var item = stack.Item;
            if (item == null) continue;

            var quantity = stack.Quantity;
            Debug.Assert(quantity > 0); // Empty item stacks should not be in container

            // Do not sell more than supply
            var itemSellCount = Math.Min(totalSellCount, quantity);

            // Prices after multiplier are rounded up.
            _gold += (int) Math.Ceiling(item.Price * _priceMultiplier) * itemSellCount;

            // Actual sell count has to be deducted from remaining sales
            _salesRemaining -= itemSellCount;

            // Sold items are no longer in container
            stack.Quantity -= itemSellCount;
            if (stack.Quantity == 0)
            {
                stack.Item = null;
            }

            itemsByPrice[index] = stack;
        }
    }

    public void SetPriceSlider(float f)
    {
        _priceMultiplier = f;
        _faithMultiplier = (float) 1.0 / f;
    }

    public int WithdrawGold()
    {
        var deducedGold = _gold;
        _gold = 0;
        return deducedGold;
    }

    public void OnEndOfDay()
    {
        _salesRemaining = MAX_SALES;
    }

    public void Test()
    {
        ItemStackController.Instance.MoveItem(ref _items[0], ref _items[1]);
    }
}