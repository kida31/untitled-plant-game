using System;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class Dehydrator : ICraftingStation
{
	//private const CraftMethod CraftMethod = Crafting.CraftMethod.Dehydrate;
	private const int SlotNumber = 6;
	private const double CraftingTime = 10;
	public event Action<ItemStack[]> RetrieveAllFinishedItemsAction;
	public event Action<ItemStack, int> ItemInserted;
	public event Action<int> ItemRemoved;
	public CraftingSlot[] CraftingSlots { get; private set; }

	private Logger _logger;

	public Dehydrator()
	{
		_logger = new Logger("Dehydrator");

		CraftingSlots = new CraftingSlot[SlotNumber];
		for (var i = 0; i < CraftingSlots.Length; i++)
		{
			CraftingSlots[i] = new CraftingSlot();
		}

		_logger.Debug($"Initialized Dehydrator with {CraftingSlots.Length} slots");
	}

	public void Process(double delta)
	{
		Assert.AssertNotNull(CraftingSlots);
		if (CraftingSlots == null) return;
		
		foreach (var slot in CraftingSlots)
		{
			slot.Process(delta);
		}
	}

	public void InsertItemToSlot(ItemStack item, int slotIndex)
	{
		_logger.Debug($"Checking slot {slotIndex} : {CraftingSlots[slotIndex].ItemStack}");
		var slot = CraftingSlots[slotIndex];
		
		if (slot.ItemStack != null)
		{
			_logger.Warn($"Slot {slotIndex} is already occupied.");
			return;
		}
		_logger.Debug($"Inserting item {item.Name} to slot {slotIndex}");

		slot.ItemStack = item;
		slot.AddItemAndStartCrafting(item, CraftingTime);
		slot.CraftTimeOut += OnCraftTimeOut; // TODO: Ready

		CraftingSlots[slotIndex] = slot;
		ItemInserted?.Invoke(item, slotIndex);
	}

	public ItemStack RemoveItemFromSlot(int slotIndex)
	{
		_logger.Debug($"Removing item from slot {slotIndex}");
		var item = CraftingSlots[slotIndex].ItemStack;
		CraftingSlots[slotIndex].RemoveItem();
		ItemRemoved?.Invoke(slotIndex);

		return item;
	}

	public void RetrieveAllFinishedItems()
	{
		for (var i = 0; i < CraftingSlots.Length; i++)
		{
			if (CraftingSlots[i].IsCraftingComplete)
			{
				RemoveItemFromSlot(i);
			}
		}
	}

	private void OnCraftTimeOut(CraftingSlot slot)
	{
		var item = slot.ItemStack;
		slot.ItemStack = ModifyItemComponent(item);
	}

	private ItemStack ModifyItemComponent(ItemStack item)
	{
		// Modify item component here
		return item;
	}
}