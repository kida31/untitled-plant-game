using System;
using Godot;
using untitledplantgame.Crafting;
using untitledplantgame.Dialogue;
using untitledplantgame.Inventory;
using untitledplantgame.Plants;
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
	public event Action OnInventoryOpen;

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

	//Dialogue

	/// <summary>
	///     Invoked when a dialogue is starting, passes the dialogue name
	/// </summary>
	public event Action<string> StartingDialogue;

	/// <summary>
	///     Emitted when a dialogue is started for the first time
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

	//Plants

	public event Action<Plant> OnSeedPlanted;

	public void SeedPlanted(Plant plant)
	{
		OnSeedPlanted?.Invoke(plant);
	}

	//HUD

	public event Action<int, int> GoldChanged;

	public void InvokeGoldChanged(int deltaGold, int newGold)
	{
		GoldChanged?.Invoke(deltaGold, newGold);
	}

	public event Action<int> OnFaithChange;
	public event Action<int> OnCurrencyChange;
	public event Action<IItemStack> OnItemPickUp;

	public void InventoryOpened()
	{
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

	public void ItemPickedUp(IItemStack item)
	{
		OnItemPickUp?.Invoke(item);
	}

	public event Action<Player.Player, IInventory> OnPlayerInventoryChanged;

	public void PlayerInventoryChanged(Player.Player player, IInventory inventory)
	{
		OnPlayerInventoryChanged?.Invoke(player, inventory);
	}

	public event Action<ICraftingStation> BeforeCraftingStationUiOpened;

	public void BeforeCraftingStationUiOpen(ICraftingStation craftingStation)
	{
		BeforeCraftingStationUiOpened?.Invoke(craftingStation);
	}
}
