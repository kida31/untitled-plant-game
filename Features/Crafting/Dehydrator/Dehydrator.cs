using System.Linq;
using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class Dehydrator : Node, ICraftingStation
{
	private const CraftMethod CraftMethod = Crafting.CraftMethod.Dehydrate;
	public CraftingSlot[] CraftingSlots { get; private set; }
	public const int SlotNumber = 6;

	public override void _Ready()
	{
		CraftingSlots = new CraftingSlot[SlotNumber];
		foreach (var slot in CraftingSlots)
		{
			slot.OnCraftingComplete += OnCraftingComplete;
		}
	}

	public void InsertItemToSlot(ItemStack item, int slotIndex)
	{
		var slot = new CraftingSlot(item, slotIndex);
		CraftingSlots[slotIndex] = slot;
		
		CraftingSlots[slotIndex].StartCrafting();
	}

	public ItemStack RemoveItemFromSlot(int slotIndex)
	{
		ItemStack item = CraftingSlots[slotIndex].ItemStack;
		CraftingSlots[slotIndex].RemoveItem();
		CraftingSlots[slotIndex] = null;
		
		return item;
	}

	public ItemStack[] GetAllItems()
	{
		return CraftingSlots.Select(slot => slot.ItemStack).ToArray();
	}

	public ItemStack[] RetrieveAllFinishedItems()
	{
		return CraftingSlots.Where(slot => slot.IsCraftingComplete).Select(slot => slot.ItemStack).ToArray();
	}

	private void OnCraftingComplete(CraftingSlot slot)
	{
		ItemStack item = CraftingSlots[slot.Index].ItemStack;
		modifyItemComponent(item);
	}

	private void modifyItemComponent(ItemStack item)
	{
		// Modify item component here
	}
}
