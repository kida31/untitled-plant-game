using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Inventory;
using untitledplantgame.Item;
using untitledplantgame.Item.Components;
using untitledplantgame.Medicine;
using MedicineComponent = untitledplantgame.Item.Components.MedicineComponent;

namespace untitledplantgame.Crafting;

public partial class Dehydrator : ICraftingStation
{
	private const int SlotNumber = 6;
	private const double CraftingTime = 10;
	private const Recipe.CraftingType CraftingType = Recipe.CraftingType.Drying;
	public event Action<ItemStack[]> RetrieveAllFinishedItemsAction;
	public event Action<ItemStack, int> ItemInserted;
	public event Action<int> ItemRemoved;
	public CraftingSlot[] CraftingSlots { get; private set; }

	private readonly Logger _logger;
	private readonly ItemDatabase _itemDatabase = ItemDatabase.Instance;
	private readonly Recipe _dryingRecipe;

	private readonly MedicineComponent _medicineComponent = new(
		new Dictionary<MedicinalEffect, int>
		{
			{ MedicinalEffect.Warming, 3 },
			{ MedicinalEffect.Antibacterial, 2 }
		},
		new Dictionary<IllnessEffect, int>
		{
			{ IllnessEffect.Indigestion, -1 }
		}
	);

	public Dehydrator()
	{
		_logger = new Logger("Dehydrator");

		CraftingSlots = new CraftingSlot[SlotNumber];
		for (var i = 0; i < CraftingSlots.Length; i++)
		{
			CraftingSlots[i] = new CraftingSlot();
		}

		_dryingRecipe = _itemDatabase.Recipes.FirstOrDefault(r => r.RecipeCraftingType == CraftingType);
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
		//var result = _dryingRecipe.CraftResult(new List<ItemStack> { item });
		var result = (ItemStack)item.Clone();
		result.AddComponent(new DriedComponent());
		result.RemoveComponent<PlantComponent>();

		ModifyComponent(result, _medicineComponent);

		result.Id += "_dried";
		result.Name = $"Dry {result.Name}";
		result.Description += " It was dried.";

		_logger.Debug($"Item modified. Resulting item: {result}");
		return result;
	}

	private void ModifyComponent<T>(ItemStack item, T component) where T : AComponent
	{
		var comp = item.GetComponent<T>();
		if (comp == null) return;

		var combinedComp = comp.Combine(component);
		
		if (combinedComp is MedicineComponent medicinalComponent && medicinalComponent.TheGoodStuff.Count == 0)
		{
			item.RemoveComponent<MedicineComponent>();
			return;
		}
		

		item.RemoveComponent<T>();
		item.AddComponent(combinedComp);
	}
}
