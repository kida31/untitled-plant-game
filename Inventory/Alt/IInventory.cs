using System;
using System.Collections;
using System.Collections.Generic;

namespace untitledplantgame.Inventory.Alt;

/// <summary>
/// https://github.com/Bukkit/Bukkit/blob/master/src/main/java/org/bukkit/inventory/Inventory.java
/// </summary>
public interface IInventory : IEnumerable<ItemStack>
{
	/**
	 * Returns the size of the inventory
	 */
	public int Size { get; }

	/**
	 * Returns the maximum stack size for an ItemStack in this inventory.
	 */
	[Obsolete]
	public int MaxStackSize { get; set; }

	/**
	 * Returns the name of the inventory
	 * 
	 * @return The String with the name of the inventory
	 */
	public string Name { get; }

	/**
	 * Returns the ItemStack found in the slot at the given index
	 * 
	 * @param index The index of the Slot's ItemStack to return
	 * @return The ItemStack in the slot
	 */
	public ItemStack GetItem(int index);

	/**
	 * Stores the ItemStack at the given index of the inventory.
	 * 
	 * @param index The index where to put the ItemStack
	 * @param item The ItemStack to set
	 */
	public void SetItem(int index, ItemStack item);

	/**
	 * Stores the given ItemStacks in the inventory. This will try to fill
	 * existing stacks and empty slots as well as it can.
	 * <p>
	 *     The returned HashMap contains what it couldn't store, where the key is
	 *     the index of the parameter, and the value is the ItemStack at that
	 *     index of the varargs parameter. If all items are stored, it will return
	 *     an empty HashMap.
	 *     <p>
	 *         If you pass in ItemStacks which exceed the maximum stack size for the
	 *         Material, first they will be added to partial stacks where
	 *         Material.getMaxStackSize() is not exceeded, up to
	 *         Material.getMaxStackSize(). When there are no partial stacks left
	 *         stacks will be split on Inventory.getMaxStackSize() allowing you to
	 *         exceed the maximum stack size for that material.
	 *         @param items The ItemStacks to add
	 *         @return A HashMap containing items that didn't fit.
	 *         @throws IllegalArgumentException if items or any element in it is null
	 */
	public Dictionary<int, ItemStack> AddItem(params ItemStack[] items);

	/**
	 * Removes the given ItemStacks from the inventory.
	 * <p>
	 *     It will try to remove 'as much as possible' from the types and amounts
	 *     you give as arguments.
	 *     <p>
	 *         The returned HashMap contains what it couldn't remove, where the key is
	 *         the index of the parameter, and the value is the ItemStack at that
	 *         index of the varargs parameter. If all the given ItemStacks are
	 *         removed, it will return an empty HashMap.
	 *         @param items The ItemStacks to remove
	 *         @return A HashMap containing items that couldn't be removed.
	 *         @throws IllegalArgumentException if items is null
	 */
	public Dictionary<int, ItemStack> RemoveItem(params ItemStack[] items);

	/**
	 * Returns all ItemStacks from the inventory
	 * 
	 * @return An array of ItemStacks from the inventory.
	 */
	public List<ItemStack> GetContents();

	/**
	 * Completely replaces the inventory's contents. Removes all existing
	 * contents and replaces it with the ItemStacks given in the array.
	 * 
	 * @param items A complete replacement for the contents; the length must
	 * be less than or equal to {@link #getSize()}.
	 * @throws IllegalArgumentException If the array has more items than the
	 * inventory.
	 */
	public void SetContents(List<ItemStack> items);


	/**
	 * Checks if the inventory contains any ItemStacks with the given
	 * id.
	 * 
	 * @param material The material to check for
	 * @return true if an ItemStack is found with the given Material
	 * @throws IllegalArgumentException if material is null
	 */
	public bool Contains(string itemId);

	/**
	 * Checks if the inventory contains any ItemStacks matching the given
	 * ItemStack.
	 * <p>
	 *     This will only return true if both the type and the amount of the stack
	 *     match.
	 *     @param item The ItemStack to match against
	 *     @return false if item is null, true if any exactly matching ItemStacks
	 *     were found
	 */
	public bool Contains(ItemStack item);

	/**
	 * Checks if the inventory contains any ItemStacks with the given
	 * material, adding to at least the minimum amount specified.
	 * 
	 * @param material The material to check for
	 * @param amount The minimum amount
	 * @return true if amount is less than 1, true if enough ItemStacks were
	 * found to add to the given amount
	 * @throws IllegalArgumentException if material is null
	 */
	public bool Contains(string itemId, int amount);

	/**
	 * Checks if the inventory contains at least the minimum amount specified
	 * of exactly matching ItemStacks.
	 * <p>
	 *     An ItemStack only counts if both the type and the amount of the stack
	 *     match.
	 *     @param item the ItemStack to match against
	 *     @param amount how many identical stacks to check for
	 *     @return false if item is null, true if amount less than 1, true if
	 *     amount of exactly matching ItemStacks were found
	 *     @see #containsAtLeast(ItemStack, int)
	 */
	public bool Contains(ItemStack item, int amount);

	/**
	 * Checks if the inventory contains ItemStacks matching the given
	 * ItemStack whose amounts sum to at least the minimum amount specified.
	 * 
	 * @param item the ItemStack to match against
	 * @param amount the minimum amount
	 * @return false if item is null, true if amount less than 1, true if
	 * enough ItemStacks were found to add to the given amount
	 */
	public bool ContainsAtLeast(ItemStack item, int amount);

	/**
	 * Returns a HashMap with all slots and ItemStacks in the inventory with
	 * the given Material.
	 * <p>
	 *     The HashMap contains entries where, the key is the slot index, and the
	 *     value is the ItemStack in that slot. If no matching ItemStack with the
	 *     given Material is found, an empty map is returned.
	 *     @param material The material to look for
	 *     @return A HashMap containing the slot index, ItemStack pairs
	 *     @throws IllegalArgumentException if material is null
	 */
	public Dictionary<int, ItemStack> All(string itemId);

	/**
	 * Finds all slots in the inventory containing any ItemStacks with the
	 * given ItemStack. This will only match slots if both the type and the
	 * amount of the stack match
	 * <p>
	 *     The HashMap contains entries where, the key is the slot index, and the
	 *     value is the ItemStack in that slot. If no matching ItemStack with the
	 *     given Material is found, an empty map is returned.
	 *     @param item The ItemStack to match against
	 *     @return A map from slot indexes to item at index
	 */
	public Dictionary<int, ItemStack> All(ItemStack item);


	/**
	 * Finds the first slot in the inventory containing an ItemStack with the
	 * given material
	 * 
	 * @param material The material to look for
	 * @return The slot index of the given Material or -1 if not found
	 * @throws IllegalArgumentException if material is null
	 */
	public int First(string itemId);

	/**
	 * Returns the first slot in the inventory containing an ItemStack with
	 * the given stack. This will only match a slot if both the type and the
	 * amount of the stack match
	 * 
	 * @param item The ItemStack to match against
	 * @return The slot index of the given ItemStack or -1 if not found
	 */
	public int First(ItemStack item);

	/**
	 * Returns the first empty Slot.
	 * 
	 * @return The first empty Slot found, or -1 if no empty slots.
	 */
	public int FirstEmpty();

	/**
	 * Removes all stacks in the inventory matching the given itemId.
	 * 
	 * @param itemId The itemId to remove
	 * @throws IllegalArgumentException if itemId is null
	 */
	public void Remove(string itemId);

	/**
	 * Removes all stacks in the inventory matching the given stack.
	 * <p>
	 *     This will only match a slot if both the type and the amount of the
	 *     stack match
	 *     @param item The ItemStack to match against
	 */
	public void Remove(ItemStack item);

	/**
	 * Clears out a particular slot in the index.
	 * 
	 * @param index The index to empty.
	 */
	public void Clear(int index);

	/**
     * Clears out the whole Inventory.
     */
	public void Clear();

	/**
	 * Returns the title of this inventory.
	 * 
	 * @return A String with the title.
	 */
	public string GetTitle();

	// /**
	//  * Returns what type of inventory this is.
	//  *
	//  * @return The InventoryType representing the type of inventory.
	//  */
	// public InventoryType getType();

	// /**
	//  * Gets the block or entity belonging to the open inventory
	//  *
	//  * @return The holder of the inventory; null if it has no holder.
	//  */
	// public InventoryHolder getHolder();


	/// <summary>
	/// Returns all ItemStacks of the given category.
	/// </summary>
	/// <param name="category"></param>
	/// <returns></returns>
	public Dictionary<int, ItemStack> GetItemsOfCategory(ItemCategory category);
}
