using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Database;
using untitledplantgame.Inventory;
using untitledplantgame.Item;
using untitledplantgame.Item.Components;
using untitledplantgame.Medicine;
using MedicineComponent = untitledplantgame.Item.Components.MedicineComponent;

namespace untitledplantgame.Crafting;

public class Dehydrator : ICraftingStation
{
	private const int SlotNumber = 6;
	private const double CraftingTime = 9; //TODO: find a good value
	private const double ValueMultiplier = 4.20; //TODO: find a good value

	private static IItemStack DriedLeaf => ItemDatabase.Instance.CreateItemStack("dried_leaf");
	private static IItemStack DriedFlower => ItemDatabase.Instance.CreateItemStack("dried_flower");
	private static IItemStack DriedFruit => ItemDatabase.Instance.CreateItemStack("dried_fruit");
	public event Action<IItemStack, int> ItemInserted;
	public event Action<bool> CraftingSlotUpdated;
	public CraftingSlot[] CraftingSlots { get;}

	private readonly Logger _logger;

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

	public void DoCraftingTickTock(double delta)
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
		if (item.Amount < 1)
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
		slot.AddItemAndStartCrafting(insertedItem, CraftingTime);
		slot.CraftTimeOut += OnCraftTimeOut; // TODO: Ready

		CraftingSlots[currentIndex] = slot;
		ItemInserted?.Invoke(insertedItem, currentIndex);
		return true;
	}
	
	private bool CheckHasFinishedItems()
	{
		return CraftingSlots.Any(slot => slot.IsCraftingComplete);
	}

	public IItemStack RemoveItemFromSlot(int slotIndex)
	{
		var item = CraftingSlots[slotIndex].ItemStack;
		_logger.Debug($"Removing item {item} from slot {slotIndex}");
		CraftingSlots[slotIndex].RemoveItem();
		CraftingSlotUpdated?.Invoke(CheckHasFinishedItems());
		return item;
	}

	public List<IItemStack> RetrieveAllFinishedItems()
	{
		var items = new List<IItemStack>();
		for (var i = 0; i < CraftingSlots.Length; i++)
		{
			if (CraftingSlots[i].IsCraftingComplete)
			{
				var item = RemoveItemFromSlot(i);
				if (item == null) continue;
				items.Add(item);
			}
		}
		CraftingSlotUpdated?.Invoke(CheckHasFinishedItems());
		return items;
	}

	private void OnCraftTimeOut(CraftingSlot slot)
	{
		var item = slot?.ItemStack;
		if (item == null)
		{
			_logger.Error("Item or slot is null.");
			return;
		}
		
		slot.ItemStack = ModifyItem(item);
		CraftingSlotUpdated?.Invoke(CheckHasFinishedItems());
	}

	private IItemStack ModifyItem(IItemStack item)
	{
		var result = ModifyComponents(item);
		
		var tags = item.GetComponent<TagsComponent>();
		if (tags.Contains(TagsComponent.Tags.IsFlower))
		{
			item.Name = $"Dried {item.Name}";
			item.Icon = DriedFlower.Icon;
			item.ToolTipDescription = DriedFlower.ToolTipDescription;
		}
		else if (tags.Contains(TagsComponent.Tags.IsFruit))
		{
			item.Name = $"Dried {item.Name}";
			item.Icon = DriedFruit.Icon;
			item.ToolTipDescription = DriedFruit.ToolTipDescription;
		}
		else
		{
			item.Name = $"Dried {item.Name}";
			item.Icon = DriedLeaf.Icon;
			item.ToolTipDescription = DriedLeaf.ToolTipDescription;
		}
		
		result.BaseValue = (int)Math.Floor(item.BaseValue * ValueMultiplier);

		_logger.Debug($"Item modified. Resulting item: {result}");
		return result;
	}

	/// <summary>
	/// Plants with Drieable Tag should always be Dried
	/// MedicineComponent only modified if medicine component exists
	/// </summary>
	/// <param name="item">Modified item</param>
	/// <returns></returns>
	private IItemStack ModifyComponents(IItemStack item)
	{
		//remove drieable
		var tags = item.GetComponent<TagsComponent>()?.Clone();

		tags.Add(TagsComponent.Tags.IsDried);
		tags.Remove(TagsComponent.Tags.IsDrieable);

		item.RemoveComponent<TagsComponent>();
		item.AddComponent(tags);

		//modify medicine component
		var comp = item.GetComponent<MedicineComponent>()?.Clone();
		if (comp == null) return item;

		foreach (var (effect, value) in _medicineComponent.TheGoodStuff)
		{
			if (comp.TheGoodStuff.ContainsKey(effect))
			{
				comp.TheGoodStuff[effect] += value;
			}
		}

		foreach (var (effect, value) in _medicineComponent.TheBadStuff)
		{
			if (comp.TheBadStuff.ContainsKey(effect))
			{
				comp.TheBadStuff[effect] += value;
			}
		}

		item.RemoveComponent<MedicineComponent>();
		item.AddComponent(comp);

		return item;
	}
}
