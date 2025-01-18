using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class DehydratorUi : Control
{
	[Export] private Button _closeButton;
	[Export] private Button _retrieveAllItemsButton;
	[Export] private GridContainer _slotContainer;

	private Dehydrator _craftingStation;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		_closeButton.Pressed += OnCraftingStationUiClosed;
		_retrieveAllItemsButton.Pressed += () =>
		{
			Assert.AssertNotNull(_craftingStation);
			_craftingStation?.RetrieveAllFinishedItems();
		};

		EventBus.Instance.BeforeCraftingStationUiOpened += BeforeCraftingStationUiOpened;
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
				slotUi.SetCraftingSlot(slots[i]);
			}
		}

		_craftingStation.ItemInserted += OnCraftingStationUiItemInserted;
		_craftingStation.ItemRemoved += OnCraftingStationUiItemRemoved;
		GameStateMachine.Instance.SetState(GameState.Crafting);
		Visible = true;
	}

	private void OnCraftingStationUiClosed()
	{
		GameStateMachine.Instance.SetState(GameState.FreeRoam);

		Visible = false;
	}

	private void OnCraftingStationUiItemInserted(IItemStack item, int slotIndex)
	{
		var slot = _craftingStation.CraftingSlots[slotIndex];
		Assert.AssertEquals(slot.ItemStack, item);
		_logger.Debug($"Setting the slot for the item");
		if (_slotContainer.GetChild(slotIndex) is CraftingSlotUi slotUi)
		{
			_logger.Debug($"The item {item.Name} should be on slot {slotIndex}. Crafting station has item {slot.ItemStack.Name}");
			slotUi.SetCraftingSlot(slot);
		}
	}

	private void OnCraftingStationUiItemRemoved(int slotIndex)
	{
		if (_slotContainer.GetChild(slotIndex) is CraftingSlotUi slot)
		{
			_logger.Debug($"Item removed from slot {slotIndex}");
			slot.SetCraftingSlot(slot.CraftingSlot);
		}
	}
}
