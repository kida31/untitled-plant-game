using System;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class Dehydrator : ICraftingStation
{
	//private const CraftMethod CraftMethod = Crafting.CraftMethod.Dehydrate;
	private const int SlotNumber = 6;
	private const double CraftingTime = 100;
	public event Action<ItemStack[]> RetrieveAllFinishedItemsAction;
	public CraftingSlot[] CraftingSlots { get; private set; }

	private Logger _logger;

	public Dehydrator()
	{
		_logger = new Logger("Dehydrator");

		CraftingSlots = new CraftingSlot[SlotNumber];

		_logger.Debug($"Initialized Dehydrator with {CraftingSlots.Length} slots");
	}

	public void Process(double delta)
	{
		if (CraftingSlots == null) return;

		foreach (var slot in CraftingSlots)
		{
			if (slot?.ItemStack == null) continue;
			slot.Process(delta);
		}
	}

	public void InsertItemToSlot(ItemStack item, int slotIndex)
	{
		var slot = CraftingSlots[slotIndex];
		_logger.Debug($"Inserting item {item.Name} to slot {slotIndex}");

		if (slot?.ItemStack != null)
		{
			_logger.Warn($"Slot {slotIndex} is already occupied.");
			return;
		}

		var newSlot = new CraftingSlot(item, slotIndex);
		newSlot.AddItem(item, CraftingTime);
		newSlot.OnCraftingComplete += OnCraftingComplete;

		CraftingSlots[slotIndex] = newSlot;
	}

	public ItemStack RemoveItemFromSlot(int slotIndex)
	{
		ItemStack item = CraftingSlots[slotIndex].ItemStack;
		CraftingSlots[slotIndex].RemoveItem();
		CraftingSlots[slotIndex] = null;

		return item;
	}

	public void RetrieveAllFinishedItems()
	{
		var items = CraftingSlots.Where(slot => slot.IsCraftingComplete).Select(slot => slot.ItemStack).ToArray();
		RetrieveAllFinishedItemsAction?.Invoke(items);
	}

	private void OnCraftingComplete(CraftingSlot slot)
	{
		var item = CraftingSlots[slot.Index].ItemStack;
		ModifyItemComponent(item);
	}

	private void ModifyItemComponent(ItemStack item)
	{
		// Modify item component here
	}
}
