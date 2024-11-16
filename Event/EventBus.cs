using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Item;

namespace untitledplantgame.Event;

/**
 * NOTE:
 *
 * Using something like "public event Action PerformedAction;" would be viable, but in order to keep things simple and
 * uniform, I decided to not use "Actions". However, this can change at any point in time if the code demands to be
 * simplified even further.
 */
public partial class EventBus : Node
{


	public static EventBus Instance { get; private set; }
	private readonly Logger _logger = new("EventBus");

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			_logger.Error("There are multiple instances of EventBus. This is not allowed. Instance will be freed.");
			QueueFree();
		}
	}

	//---------------------------------------------Legacy Signals---------------------------------------------
	[Signal]
	[Obsolete]
	public delegate void NPCInteractedEventHandler(Node npc); //Replace with C# Action

	[Obsolete]
	public void NotifyNPCInteracted(Node npc)
	{
		EmitSignal(nameof(NPCInteracted), npc);
	}
	//---------------------------------------------Legacy Signals---------------------------------------------
	
	public event Action<ItemStack> OnItemPickUp;

	public void ItemPickedUp(ItemStack item)
	{
		OnItemPickUp?.Invoke(item);
	}


	public delegate void UpdateTabsInventoryEventHandler(ItemStack item);

	public event UpdateTabsInventoryEventHandler OnTabsUpdated;

	public void TabsUpdated(ItemStack item)
	{
		OnTabsUpdated?.Invoke(item);
	}
	
	
	public delegate void UpdateInventoryItemPosition(ItemStack itemStack, InventoryItemView inventoryItemView);
	public event UpdateInventoryItemPosition OnInventoryItemMoved;
	
	public void InventoryItemMoved(ItemStack itemStack, InventoryItemView inventoryItemView)
	{
		OnInventoryItemMoved?.Invoke(itemStack, inventoryItemView);
	}

	
	
	public delegate void UpdateItemSlotEventHandler(InventoryItemView inventoryItemView);
	public event UpdateItemSlotEventHandler OnSetItemSlot;

	public void SetItemSlot(InventoryItemView inventoryItemView)
	{
		OnSetItemSlot?.Invoke(inventoryItemView);
	}
	
	
	public delegate InventoryItemView GetItemSlotEventHandler();
	public event GetItemSlotEventHandler OnGetItemSlot;

	public InventoryItemView GetItemSlot()
	{
		return OnGetItemSlot?.Invoke();
	}


	public delegate void UpdatedDetailedItemView(Texture2D icon, string description);
	public event UpdatedDetailedItemView OnItemClicked;

	public void UiItemClicked(Texture2D icon, string description)
	{
		OnItemClicked?.Invoke(icon, description);
	}
	
	
}
