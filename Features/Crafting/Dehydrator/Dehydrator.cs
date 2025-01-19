using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.Database;
using untitledplantgame.Inventory;
using untitledplantgame.Item.Components;
using untitledplantgame.Medicine;
using MedicineComponent = untitledplantgame.Item.Components.MedicineComponent;

namespace untitledplantgame.Crafting;

public class Dehydrator : ICraftingStation
{
	private const int SlotNumber = 6;
	private const double CraftingTime = 10; //TODO: find a good value
	private const Recipe.CraftingType CraftingType = Recipe.CraftingType.Drying;
	private const double ValueMultiplier = 4.20; //TODO: find a good value
	
	private static readonly IItemStack DriedLeaf = ItemDatabase.Instance.CreateItemStack("dried_leaf");
	private static readonly IItemStack DriedFlower = ItemDatabase.Instance.CreateItemStack("dried_flower");
	private static readonly IItemStack DriedFruit = ItemDatabase.Instance.CreateItemStack("dried_fruit");
	
	public event Action<IItemStack[]> RetrieveAllFinishedItemsAction; //TODO: add items to inventory
	public event Action<IItemStack, int> ItemInserted;
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

	public void InsertItemToSlot(IItemStack item, int slotIndex)
	{
		_logger.Debug($"Checking slot {slotIndex} : {CraftingSlots[slotIndex].ItemStack}");
		var slot = CraftingSlots[slotIndex];
		
		var tags = item.GetComponent<TagsComponent>();
		if (!tags.Contains(TagsComponent.Tags.IsDrieable)) return;

		if (slot.ItemStack != null)
		{
			_logger.Warn($"Slot {slotIndex} is already occupied.");
			return;
		}

		_logger.Debug($"Inserting item {item.Name} to slot {slotIndex}");
		var insertedItem = item.Clone();
		
		insertedItem.Amount = 1;
		slot.ItemStack = insertedItem;
		slot.AddItemAndStartCrafting(item, CraftingTime);
		slot.CraftTimeOut += OnCraftTimeOut; // TODO: Ready

		CraftingSlots[slotIndex] = slot;
		ItemInserted?.Invoke(item, slotIndex);
	}

	public IItemStack RemoveItemFromSlot(int slotIndex)
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
		slot.ItemStack = ModifyItem(item);
	}

	private IItemStack ModifyItem(IItemStack item)
	{
		var result = ModifyComponent(item);
		
		//Get Item Template based on whether it's a Leaf, Flower or Fruit
		result.BaseValue = (int) Math.Floor(item.BaseValue * ValueMultiplier);

		_logger.Debug($"Item modified. Resulting item: {result}");
		return result;
	}

	private IItemStack ModifyComponent(IItemStack item)
	{
		var comp = item.GetComponent<MedicineComponent>().Clone();
		if (comp == null) return item;

		foreach (var (effect, value) in _medicineComponent.TheGoodStuff)
		{
			if (!comp.TheGoodStuff.ContainsKey(effect))
			{
				comp.TheGoodStuff[effect] += value;
			}
		}
		foreach (var (effect, value) in _medicineComponent.TheBadStuff)
		{
			if (!comp.TheBadStuff.ContainsKey(effect))
			{
				comp.TheBadStuff[effect] += value;
			}
		}
		

		item.RemoveComponent<MedicineComponent>();
		item.AddComponent(comp);
		
		var tags = item.GetComponent<TagsComponent>().Clone();
		tags.Add(TagsComponent.Tags.IsDried);
		tags.Remove(TagsComponent.Tags.IsDrieable);
		
		item.RemoveComponent<TagsComponent>();
		item.AddComponent(tags);
		
		return item;
	}
}
