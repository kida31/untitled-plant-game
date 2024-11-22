using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.EntityStatsDataContainer;
using untitledplantgame.Item;
using untitledplantgame.Shops;
using untitledplantgame.VendingMachine;

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



	public delegate void AddToInventoryEventHandler(InteractableItem interactableItem);

	public event AddToInventoryEventHandler OnItemPickUp;

	public void ItemPickedUp(InteractableItem interactableItem)
	{
		OnItemPickUp?.Invoke(interactableItem);
	}

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

	public event Action<VendingMachine> BeforeVendingMachineOpened;

	public void BeforeVendingMachineOpen(VendingMachine vendingMachine)
	{
		BeforeVendingMachineOpened?.Invoke(vendingMachine);
	}
}
