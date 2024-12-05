using System;
using Godot;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Dialogue;
using untitledplantgame.Inventory;
using untitledplantgame.Inventory.PlayerInventory.UI_InventoryItem;
using untitledplantgame.Inventory.PlayerInventory.UI_Wiki;
using untitledplantgame.Shops;

namespace untitledplantgame.Common;

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


	

	public event Action OnSeedshopOpened;

	public void SeedshopOpened()
	{
		OnSeedshopOpened?.Invoke();
	}

	public event Action<IShop> OnSeedShopOpening;

	public void SeedShopOpening(IShop shop)
	{
		OnSeedShopOpening?.Invoke(shop);
	}

	public event Action OnSeedshopClosed;

	public void SeedshopClosed()
	{
		OnSeedshopClosed?.Invoke();
	}

	public event Action<VendingMachine.VendingMachine> BeforeVendingMachineOpened;

	public void BeforeVendingMachineOpen(VendingMachine.VendingMachine vendingMachine)
	{
		BeforeVendingMachineOpened?.Invoke(vendingMachine);
	}

	/// <summary>
	/// Invoked when a dialogue is starting, passes the dialogue name
	/// </summary>
	public event Action<string> StartingDialogue;

	/// <summary>
	/// Emitted when a dialogue is started for the first time
	/// </summary>
	public event Action<IDialogueSystem> InitialiseDialogue;

	public void InvokeStartingDialogue(string obj)
	{
		StartingDialogue?.Invoke(obj);
	}

	public void InvokeInitialiseDialogue(IDialogueSystem obj)
	{
		InitialiseDialogue?.Invoke(obj);
	}

	public event Action<int, int> GoldChanged;
	public void InvokeGoldChanged(int deltaGold, int newGold)
	{
		GoldChanged?.Invoke(deltaGold, newGold);
	}
	
	public delegate InventoryItemView GetItemSlotEventHandler();

	public Action OnInventoryOpen;
	public event Action<int> OnFaithChange;
	public event Action<int> OnCurrencyChange;
	public event Action<WikiItemView> OnScrollContainerViewUpdate; 
	public event Action<ItemStack> OnTabsUpdate;
	public event Action<ItemStack> OnItemPickUp;
	public event Action<ItemStack> OnWikiItemClicked;
	public event Action<InventoryItemView> OnInventoryItemViewPressed;
	public event Action<InventoryItemView> OnInventoryItemViewMoved;
	public event Action<InventoryItemView> OnInventoryItemViewReleased;
	public event Action<InventoryItemView> OnSetItemSlot;
	public event Action<IItemStack, InventoryItemView> OnInventoryItemMove;
	public event GetItemSlotEventHandler OnGetItemSlot;

	public void InventoryOpened()
	{
		GameStateMachine.Instance.SetState(GameState.Book);
		OnInventoryOpen?.Invoke();
	}
	
	public void FaithChanged(int change)
	{
		OnFaithChange?.Invoke(change);
	}
	
	public void CurrencyChanged(int change)
	{
		OnCurrencyChange?.Invoke(change);
	}

	public void ScrollContainerViewChanged(WikiItemView scrollContainerElement)
	{
		OnScrollContainerViewUpdate?.Invoke(scrollContainerElement);
	}
	
	public void TabsUpdated(ItemStack item)
	{
		OnTabsUpdate?.Invoke(item);
	}
	
	public void ItemPickedUp(ItemStack item)
	{
		OnItemPickUp?.Invoke(item);
	}
	
	public void UiWikiItemClicked(ItemStack itemStack)
	{
		OnWikiItemClicked?.Invoke(itemStack);
	}

	public void UiInventoryItemViewPressed(InventoryItemView inventoryItemView)
	{
		OnInventoryItemViewPressed?.Invoke(inventoryItemView);
	}
	
	public void UiInventoryItemViewMoved(InventoryItemView inventoryItemView)
	{
		OnInventoryItemViewMoved?.Invoke(inventoryItemView);
	}
	
	public void UiInventoryItemViewReleased(InventoryItemView inventoryItemView)
	{
		OnInventoryItemViewReleased?.Invoke(inventoryItemView);
	}
	
	public void SetItemSlot(InventoryItemView inventoryItemView)
	{
		OnSetItemSlot?.Invoke(inventoryItemView);
	}
	
	public void InventoryItemMoved(IItemStack itemStack, InventoryItemView inventoryItemView)
	{
		OnInventoryItemMove?.Invoke(itemStack, inventoryItemView);
	}

	public InventoryItemView GetItemSlot()
	{
		return OnGetItemSlot?.Invoke();
	}
	
	public event Action<Player.Player, IInventory> OnPlayerInventoryChanged;
	public void PlayerInventoryChanged(Player.Player player, IInventory inventory)
	{
		OnPlayerInventoryChanged?.Invoke(player, inventory);
	}
}
