using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.GUI.Items;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class DehydratorInventoryView : Control
{
	[Export] private CraftingInventory _playerInventory;
	[Export] private DehydratorUi _dehydratorUi;

	private int _index;
	private Dehydrator _dehydrator;

	public override void _Ready()
	{
		base._Ready();
		//_playerInventory.InsertingItemToCraftingStation += OnInsertingItem;
	}

	private void OnInsertingItem(NewInventoryItemView obj)
	{
		//get first empty slot of dehydrator
		_index = -1;
		for (var i = 0; i < _dehydrator.CraftingSlots.Length; i++)
		{
			if (_dehydrator.CraftingSlots[i].ItemStack != null)
			{
				continue;
			}

			_index = i;
			break;
		}
		//_dehydrator.InsertItemToSlot(obj.ItemStack, _index);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey)
		{
			if(@event.IsPressed() && ((InputEventKey)@event).Keycode == Key.F1)
			{
				OpenDehydratorMenu();
			}
		} 
	}

	public void OpenDehydratorMenu()
	{
		GameStateMachine.Instance.ChangeState(GameState.Crafting);
		_dehydrator ??= new Dehydrator();
		_dehydratorUi.Show();
		_playerInventory.ShowInventory(Game.Player.Inventory.GetInventory(ItemCategory.Medicine));
		Show();
	}
}
