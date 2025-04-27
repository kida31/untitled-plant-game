using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Quests;

[GlobalClass]
public partial class AddItemToVMTask : QuestTask
{
	[Export] private string _itemId;
	[Export] private int _amount;
	public override bool IsCompleted => _isCompleted;
	public override event Action<QuestTask> TaskCompleted;
	
	private bool _isCompleted = false;
	private int _addedAmount = 0;
	
	public AddItemToVMTask()
	{
		EventBus.Instance.ItemAddedToVendingMachine += OnItemAddedToVendingMachine;
	}

	private void OnItemAddedToVendingMachine(IItemStack obj)
	{
		if (obj.Id != _itemId)
		{
			return;
		}
		
		_addedAmount += obj.Amount;
		if (_addedAmount < _amount)
		{
			return;
		}

		_isCompleted = true;
		
		EventBus.Instance.ItemAddedToVendingMachine -= OnItemAddedToVendingMachine;
		TaskCompleted?.Invoke(this);
	}
}
