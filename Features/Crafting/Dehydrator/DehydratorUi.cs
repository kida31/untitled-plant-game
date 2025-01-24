using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Common.Inputs.GameActions;
using untitledplantgame.GUI.Items;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class DehydratorUi : Control
{
	[Export] private Button _retrieveAllItemsButton;
	[Export] private GridContainer _slotContainer;
	[Export] private CraftingInventory _playerInventory;

	private Dehydrator _craftingStation;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		_retrieveAllItemsButton.Pressed += () =>
		{
			Assert.AssertNotNull(_craftingStation);
			
			var items = _craftingStation?.RetrieveAllFinishedItems();
			var itemsArr = items?.Where(i => i != null).ToArray();
			if(itemsArr != null) Game.Player.Inventory.AddItem(itemsArr);
			
			for (var i = 0; i < _slotContainer.GetChildCount(); i++)
			{
				OnCraftingStationUiItemRemoved(i);
			}
		};

		EventBus.Instance.BeforeCraftingStationUiOpened += BeforeCraftingStationUiOpened;
		_playerInventory.RemovingItemFromInventory += OnInventoryWantsToDoSomethingWithItem;
		var slotUi = _slotContainer.GetChildren();
		for (var index = 0; index < slotUi.Count; index++)
		{
			var slot = slotUi[index];
			if (slot is CraftingSlotUi slotUiInstance)
			{
				var i = index;
				slotUiInstance.Pressed += () => PressedCraftingSlot(slotUiInstance, i);
			}
		}
	}

	private void OnInventoryWantsToDoSomethingWithItem(NewInventoryItemView obj)
	{
		if(obj.ItemStack == null) return;
		var wasInserted = _craftingStation.InsertItemToSlot(obj.ItemStack);
		if (!wasInserted) return;

		obj.Inventory.PopItemFromSlot(obj.SlotIndex, 1);
	}

	private void BeforeCraftingStationUiOpened(ICraftingStation dehydrator)
	{
		_logger.Debug("Opening Dehydrator UI");
		_craftingStation = dehydrator as Dehydrator;
		if (_craftingStation == null)
		{
			_logger.Error("Dehydrator is null");
			return;
		}

		var slots = _craftingStation.CraftingSlots;
		var uiSlots = _slotContainer.GetChildren();
		Assert.AssertTrue(0 < slots.Length);
		Assert.AssertTrue(uiSlots.Count > 0);
		Assert.AssertTrue(slots.Length == uiSlots.Count);
		for (var i = 0; i < slots.Length; i++)
		{
			Assert.AssertNotNull(slots[i]);
			if (slots[i] == null)
			{
				_logger.Error("Slot is null");
				continue;
			}

			Assert.AssertTrue(uiSlots[i] is CraftingSlotUi, "this should be a crafting slot uwu");
			if (uiSlots[i] is CraftingSlotUi slotUi)
			{
				slotUi.UpdateCraftingSlot(slots[i]);
			}
		}

		_craftingStation.ItemInserted += OnCraftingStationUiItemInserted;
		GameStateMachine.Instance.SetState(GameState.Crafting);
		
		_playerInventory.ShowInventory(Game.Player.Inventory.GetInventory(ItemCategory.Medicine));
		_playerInventory.Show();
		Show();
		_playerInventory.GrabFocus(); // Grab primary focus
	}

	private void PressedCraftingSlot(CraftingSlotUi slot, int index)
	{
		if(slot.ItemStack == null) return;
		Game.Player.Inventory.AddItem(_craftingStation.RemoveItemFromSlot(index));
		OnCraftingStationUiItemRemoved(index);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		base._UnhandledInput(@event);
		if (@event.IsActionPressed(UINavigation.Cancel) && IsVisibleInTree())
		{
			OnCraftingStationUiClosed();
		}
	}

	private void OnCraftingStationUiClosed()
	{
		GameStateMachine.Instance.SetState(GameState.FreeRoam);
		Hide();
		_playerInventory.Hide();
	}

	private void OnCraftingStationUiItemInserted(IItemStack item, int slotIndex)
	{
		var slot = _craftingStation.CraftingSlots[slotIndex];
		Assert.AssertEquals(slot.ItemStack, item);
		_logger.Debug($"Setting the slot for the item");
		if (_slotContainer.GetChild(slotIndex) is CraftingSlotUi slotUi)
		{
			_logger.Debug($"The item {item.Name} should be on slot {slotIndex}. Crafting station has item {slot.ItemStack.Name}");
			slotUi.UpdateCraftingSlot(slot);
		}
	}

	private void OnCraftingStationUiItemRemoved(int slotIndex)
	{
		if (_slotContainer.GetChild(slotIndex) is CraftingSlotUi slot)
		{
			_logger.Debug($"Item removed from slot {slotIndex}");
			slot.UpdateCraftingSlot(slot.CraftingSlot);
		}
	}
}
