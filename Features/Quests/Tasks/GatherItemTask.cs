using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Quests;

[GlobalClass]
public partial class GatherItemTask : QuestTask
{
	[Export] private string _itemId;
	[Export] private int _amount;
	
	public override bool IsCompleted => _isCompleted;
	public override event Action<QuestTask> TaskCompleted;
	private bool _isCompleted = false;
	private int _gatheredAmount = 0;

	public override void StartTask()
	{
		EventBus.Instance.OnItemAddedToInventory += OnItemAddedToInventory;
	}

	protected override void StopTask()
	{
		EventBus.Instance.OnItemAddedToInventory -= OnItemAddedToInventory;
		TaskCompleted?.Invoke(this);
	}

	private void OnItemAddedToInventory(IItemStack obj)
	{
		if (obj.Id != _itemId)
		{
			return;
		}
		
		_gatheredAmount++;
		if (_gatheredAmount < _amount)
		{
			return;
		}

		_isCompleted = true;
		
		StartTask();
	}
}
