using System.Linq;
using Godot;
using untitledplantgame.Inventory;
using untitledplantgame.Item;

namespace untitledplantgame.Crafting;

public partial class Dehydrator : Node, ICraftingStation
{
	private const CraftMethod CraftMethod = Crafting.CraftMethod.Dehydrate;
	
	private int _slotNumber = 6;
	private CraftingSlot[] _craftingSlots;
	private IItemDatabase _itemDatabase;

	public override void _Ready()
	{
		_craftingSlots = new CraftingSlot[_slotNumber];
		foreach (var slot in _craftingSlots)
		{
			slot.OnCraftingComplete += OnCraftingComplete;
		}
	}

	public void InsertItemToSlot(ItemStack item, int slotIndex)
	{
		_craftingSlots[slotIndex].ItemStack = item;
		_craftingSlots[slotIndex].StartCrafting();
	}

	public ItemStack RemoveItemFromSlot(int slotIndex)
	{
		ItemStack item = _craftingSlots[slotIndex].ItemStack;
		_craftingSlots[slotIndex].RemoveItem();
		return item;
	}

	public ItemStack[] GetAllItemSlots()
	{
		return _craftingSlots.Select(slot => slot.ItemStack).ToArray();
	}

	public ItemStack[] RetrieveAllFinishedItems()
	{
		return _craftingSlots.Where(slot => slot.IsCraftingComplete).Select(slot => slot.ItemStack).ToArray();
	}

	private void OnCraftingComplete(ItemStack item)
	{
		ItemStack result;
		var resultResource = _itemDatabase.RequestCraftedItem(item, CraftMethod);
	}
}
