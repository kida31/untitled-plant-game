using System;
using Godot;
using untitledplantgame.Crafting;
using untitledplantgame.Dialogue;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.Inventory;
using untitledplantgame.Plants;
using untitledplantgame.Shops;
using untitledplantgame.Vending;

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
	
	public static bool DisplayLog; // TODO: This may or may not be a good place to store this variable...
	
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

	public event Action<VendingMachine> BeforeVendingMachineOpened;

	public void BeforeVendingMachineOpen(VendingMachine vendingMachine)
	{
		BeforeVendingMachineOpened?.Invoke(vendingMachine);
	}

	//Dialogue

	/// <summary>
	///     Invoked when a dialogue is starting, passes the dialogue object
	/// </summary>
	public event Action<DialogueResourceObject> StartingDialogue;

	/// <summary>
	///     Emitted when a dialogue is started, passes the dialogue system
	/// </summary>
	public event Action<IDialogueSystem> InitialiseDialogue;

	public void InvokeStartingDialogue(DialogueResourceObject obj)
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
	
	// An event to change the portrait! Shouldn't be hard. But I don't know how to translate them into emotions
	public event Action<AnimatedSprite2D, string> OnNpcStartDialogue;
	
	public void InvokeGoldChanged(int deltaGold, int newGold)
	{
		GoldChanged?.Invoke(deltaGold, newGold);
	}


	public void NpcDialogueWithPlayerStarted(AnimatedSprite2D portrait, string npcName)
	{
		OnNpcStartDialogue?.Invoke(portrait, npcName);
	}
	
	
	//Inventory

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
