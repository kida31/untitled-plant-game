using System;
using System.Collections.Generic;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory;

// If any methods/handlers are missing, please contact @kida31
// or... well don't. Just add it yourself.
/// <summary>
///     https://github.com/Bukkit/Bukkit/blob/master/src/main/java/org/bukkit/inventory/Inventory.java
/// </summary>
public interface IInventory : IEnumerable<IItemStack>
{
	// TODO verify if these events make sense
	public event Action InventoryChanged;
	public event Action<IItemStack> ItemAdded;
	public event Action<IItemStack> ItemRemoved;
	
	/// <summary>
	///     Returns the size of the inventory
	/// </summary>
	public int Size { get; }

	/// <summary>
	///     Returns the name of the inventory
	/// </summary>
	public string Name { get; }

	/// <summary>
	///     Returns the IItemStack found in the slot at the given index
	/// </summary>
	/// <param name="index">The index of the Slot's IItemStack to return</param>
	/// <returns>The IItemStack in the slot</returns>
	public IItemStack GetItem(int index);

	/// <summary>
	///     Stores the IItemStack at the given index of the inventory.
	/// </summary>
	/// <param name="index">The index where to put the IItemStack</param>
	/// <param name="item">The IItemStack to set</param>
	public void SetItem(int index, IItemStack item);

	/// <summary>
	///     Stores the given IItemStacks in the inventory. This will try to fill
	///     existing stacks and empty slots as well as it can.
	///     <para>
	///         The returned HashMap contains what it couldn't store, where the key is
	///         the index of the parameter, and the value is the IItemStack at that
	///         index of the varargs parameter. If all items are stored, it will return
	///         an empty HashMap.
	///         <para>
	///             If you pass in IItemStacks which exceed the maximum stack size for the
	///             Material, first they will be added to partial stacks where
	///             Material.getMaxStackSize() is not exceeded, up to
	///             Material.getMaxStackSize(). When there are no partial stacks left
	///             stacks will be split on Inventory.getMaxStackSize() allowing you to
	///             exceed the maximum stack size for that material.
	///             @throws IllegalArgumentException if items or any element in it is null
	/// </para>
	/// </para>
	/// </summary>
	/// <param name="items">The IItemStacks to add</param>
	/// <returns>A HashMap containing items that didn't fit.</returns>
	public Dictionary<int, IItemStack> AddItem(params IItemStack[] items);

	/// <summary>
	///     Removes the given IItemStacks from the inventory.
	///     <para>
	///         It will try to remove 'as much as possible' from the types and amounts
	///         you give as arguments.
	///
	///         <para>
	///             The returned HashMap contains what it couldn't remove, where the key is
	///             the index of the parameter, and the value is the IItemStack at that
	///             index of the varargs parameter. If all the given IItemStacks are
	///             removed, it will return an empty HashMap.
	///             @throws IllegalArgumentException if items is null
	/// </para>
	/// </para>
	/// </summary>
	/// <param name="items">The IItemStacks to remove</param>
	/// <returns>A HashMap containing items that couldn't be removed.</returns>
	public Dictionary<int, IItemStack> RemoveItem(params IItemStack[] items);

	/// <summary>
	///     Returns all IItemStacks from the inventory
	/// </summary>
	/// <returns>An array of IItemStacks from the inventory.</returns>
	public List<IItemStack> GetItems();

	/// <summary>
	///     Completely replaces the inventory's contents. Removes all existing
	///     contents and replaces it with the IItemStacks given in the array.
	///     @throws IllegalArgumentException If the array has more items than the
	///     inventory.
	/// </summary>
	/// <param name="items">A complete replacement for the contents; the length must
	///     be less than or equal to Size.</param>
	public void SetContents(List<IItemStack> items);

	/// <summary>
	///     Checks if the inventory contains any IItemStacks with the given
	///     id.
	///     @throws IllegalArgumentException if material is null
	/// </summary>
	/// <param name="itemId">The material to check for</param>
	/// <returns>true if an IItemStack is found with the given Material</returns>
	public bool Contains(string itemId);

	/// <summary>
	///     Checks if the inventory contains any IItemStacks matching the given
	///     IItemStack.
	///     <para>
	///         This will only return true if both the type and the amount of the stack
	///         match.
	/// </para>
	/// </summary>
	/// <param name="item">item The IItemStack to match against</param>
	/// <returns> false if item is null, true if any exactly matching IItemStacks were found</returns>
	public bool Contains(IItemStack item);

	/// <summary>
	///     Checks if the inventory contains any IItemStacks with the given
	///     material, adding to at least the minimum amount specified.
	///     @throws IllegalArgumentException if material is null
	/// </summary>
	/// <param name="itemId">The material to check for</param>
	/// <param name="amount">The minimum amount</param>
	/// <returns>true if amount is less than 1, true if enough IItemStacks were
	///     found to add to the given amount</returns>
	public bool Contains(string itemId, int amount);

	/// <summary>
	///     Checks if the inventory contains at least the minimum amount specified
	///     of exactly matching IItemStacks.
	///     <para>
	///         An IItemStack only counts if both the type and the amount of the stack
	///         match.
	/// </para>
	/// </summary>
	/// <param name="item">the IItemStack to match against</param>
	/// <param name="amount">how many identical stacks to check for</param>
	/// <returns>false if item is null, true if amount less than 1, true if
	///         amount of exactly matching IItemStacks were found</returns>
	public bool Contains(IItemStack item, int amount);

	/// <summary>
	///     Returns a HashMap with all slots and IItemStacks in the inventory with
	///     the given Material.
	///     <para>
	///         The HashMap contains entries where, the key is the slot index, and the
	///         value is the IItemStack in that slot. If no matching IItemStack with the
	///         given Material is found, an empty map is returned.
	///         @throws IllegalArgumentException if material is null
	///     </para>
	/// </summary>
	/// <param name="itemId">The material to look for</param>
	/// <returns>A HashMap containing the slot index, IItemStack pairs</returns>
	public Dictionary<int, IItemStack> All(string itemId);

	/// <summary>
	///     Finds all slots in the inventory containing any IItemStacks with the
	///     given IItemStack. This will only match slots if both the type and the
	///     amount of the stack match
	///     <para>
	///         The HashMap contains entries where, the key is the slot index, and the
	///         value is the IItemStack in that slot. If no matching IItemStack with the
	///         given Material is found, an empty map is returned.
	/// </para>
	/// </summary>
	/// <param name="item">The IItemStack to match against</param>
	/// <returns>A map from slot indexes to item at index</returns>
	public Dictionary<int, IItemStack> All(IItemStack item);

	/// <summary>
	///     Finds the first slot in the inventory containing an IItemStack with the
	///     given material
	///     @throws IllegalArgumentException if material is null
	/// </summary>
	/// <param name="itemId">The material to look for</param>
	/// <returns>The slot index of the given Material or -1 if not found</returns>
	public int First(string itemId);

	/// <summary>
	///     Returns the first slot in the inventory containing an IItemStack with
	///     the given stack. This will only match a slot if both the type and the
	///     amount of the stack match
	/// </summary>
	/// <param name="item"> The IItemStack to match against</param>
	/// <returns>The slot index of the given IItemStack or -1 if not found</returns>
	public int First(IItemStack item);

	/// <summary>
	///     Removes all stacks in the inventory matching the given itemId.
	///     @throws IllegalArgumentException if itemId is null
	/// </summary>
	/// <param name="itemId">The itemId to remove</param>
	public void RemoveAll(string itemId);

	/// <summary>
	///     Removes all stacks in the inventory matching the given stack.
	///     <para>
	///         This will only match a slot if both the type and the amount of the
	///         stack match
	/// </para>
	/// </summary>
	/// <param name="item">The IItemStack to match against</param>
	public void RemoveAll(IItemStack item);

	/// <summary>
	///     Clears out a particular slot in the index.
	/// </summary>
	/// <param name="index">The index to empty.</param>
	public void Clear(int index);

	/// <summary>
	///     Clears out the whole Inventory.
	/// </summary>
	public void Clear();

	// /// <summary>
	//  /// Returns what type of inventory this is.
	//  ///
	//  /// @return The InventoryType representing the type of inventory.
	//  /// </summary>
	// public InventoryType getType();

	// /// <summary>
	//  /// Gets the block or entity belonging to the open inventory
	//  ///
	//  /// @return The holder of the inventory; null if it has no holder.
	//  /// </summary>
	// public InventoryHolder getHolder();


	/// <summary>
	///     Returns all IItemStacks of the given category.
	/// </summary>
	/// <param name="category"></param>
	/// <returns></returns>
	public Dictionary<int, IItemStack> GetItemsOfCategory(ItemCategory category);

	/// <summary>
	///     Tries to quick stack items from this inventory to the target inventory.
	///		Only items that already exist in the target inventory will be stacked.
	/// </summary>
	/// <param name="target"></param>
	void QuickStack(IInventory target);

	void TransferTo(IItemStack item, IInventory destination)
	{
		Assert.AssertTrue(Contains(item), "Should not try to transfer items that are larger than the available quantity");
		RemoveItem(item);
		destination.AddItem(item);
	}

	/// <summary>
	///     Adds an item to the specified slot and returns the remaining stack that could not be added.
	/// <para>
	/// If the slot index is invalid, the item will not be added and the item will be returned.
	/// If the destination item stack is of different type, all items will be returned as well.
	/// </para>
	/// </summary>
	/// <param name="slotIdx"></param>
	/// <param name="item"></param>
	/// <returns></returns>
	IItemStack AddItemToSlot(int slotIdx, IItemStack item);
}
// TODO: Replace javadoc @tags with c# xml variant
