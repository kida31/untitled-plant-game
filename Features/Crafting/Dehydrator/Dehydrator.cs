using System;
using System.Collections.Generic;
using Godot;
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
	private const double CraftingTime = 9; //TODO: find a good value
	private const Recipe.CraftingType CraftingType = Recipe.CraftingType.Drying;
	private const double ValueMultiplier = 4.20; //TODO: find a good value

	private static readonly IItemStack DriedLeaf = ItemDatabase.Instance.CreateItemStack("dried_leaf");
	private static readonly IItemStack DriedFlower = ItemDatabase.Instance.CreateItemStack("dried_flower");
	private static readonly IItemStack DriedFruit = ItemDatabase.Instance.CreateItemStack("dried_fruit");

	public string ActionName { get; } = "Dehydrate";
	public event Action<IItemStack, int> ItemInserted;
	public event Action<int> ItemRemoved;
	public CraftingSlot[] CraftingSlots { get; private set; }

	private readonly Logger _logger;
	private readonly ItemDatabase _itemDatabase = ItemDatabase.Instance;

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

	/// <summary>
	/// Insert an item to the first empty slot. If the item is not drieable, return false. If successful, return true.
	/// Successful means the item is inserted to the first empty slot and there is at least one empty slot.
	/// Item is cloned and will not be modified.
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public bool InsertItemToSlot(IItemStack item)
	{
		//get first empty CraftingSlot
		var currentIndex = -1;
		for (var i = 0; i < CraftingSlots.Length; i++)
		{
			if (CraftingSlots[i].ItemStack != null)
			{
				continue;
			}
			currentIndex = i;
			break;
		}

		if (currentIndex == -1) return false;
		
		var slot = CraftingSlots[currentIndex];
		if(item.Amount < 1)
		{
			_logger.Error("Cannot insert item with amount less than 1.");
			return false;
		}
		
		var tags = item.GetComponent<TagsComponent>();
		if (!tags.Contains(TagsComponent.Tags.IsDrieable)) return false;

		if (slot.ItemStack != null)
		{
			_logger.Error($"Slot {currentIndex} is already occupied.");
			return false;
		}

		_logger.Debug($"Inserting item {item.Name} to slot {currentIndex}");
		var insertedItem = item.Clone();
		insertedItem.Amount = 1;
		
		slot.ItemStack = insertedItem;
		slot.AddItemAndStartCrafting(item, CraftingTime);
		slot.CraftTimeOut += OnCraftTimeOut; // TODO: Ready

		CraftingSlots[currentIndex] = slot;
		ItemInserted?.Invoke(item, currentIndex);
		return true;
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
		result.BaseValue = (int)Math.Floor(item.BaseValue * ValueMultiplier);

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
