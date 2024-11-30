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
		_craftingStation = dehydrator as Dehydrator;
		if (_craftingStation == null)
		{
			_logger.Error("Dehydrator is null");
			return;
		}

		var slots = _craftingStation.CraftingSlots;
		var prefab = GD.Load<PackedScene>("res://Features/Crafting/crafting_slot.tscn");
		foreach (var craftingSlot in slots)
		{
			if(craftingSlot == null)
				continue;
			var slot = prefab.Instantiate<CraftingSlotUi>();
			// TODO: slot._craftingSlot = craftingSlot;
			_slotContainer.AddChild(slot);
		}
		GameStateMachine.Instance.SetState(GameState.Crafting);
		Visible = true;
	}

	private void OnCraftingStationUiClosed()
	{
		GameStateMachine.Instance.SetState(GameState.FreeRoam);
		Visible = false;
	}

	private void OnCraftingStationUiItemInserted(ItemStack item, int slotIndex)
	{
		_craftingStation.InsertItemToSlot(item, slotIndex);
		if (_slotContainer.GetChild(slotIndex) is CraftingSlotUi { ItemStack: null } slot)
		{
			slot.ItemStack = item;
		}
	}

	private void OnCraftingStationUiItemRemoved(int slotIndex)
	{
		var item = _craftingStation.RemoveItemFromSlot(slotIndex);
		if (_slotContainer.GetChild(slotIndex) is CraftingSlotUi slot && slot.ItemStack == item)
		{
			slot.ItemStack = null;
		}
	}
}
