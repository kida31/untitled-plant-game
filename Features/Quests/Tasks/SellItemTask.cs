using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Inventory;

namespace untitledplantgame.Quests;

[GlobalClass]
public partial class SellItemTask : QuestTask
{
	[Export] private string _itemId;
	[Export] private int _amount;
	public override bool IsCompleted => _isCompleted;
	public override event Action<QuestTask> TaskCompleted;
	
	private bool _isCompleted = false;
	private int _soldCount = 0;

	private void _Ready()
	{
		EventBus.Instance.ItemSoldFromVendingMachine += OnItemSold;
	}

	private void OnItemSold(IItemStack obj)
	{
		if (obj.Id != _itemId)
		{
			return;
		}

		_soldCount++;
		if(_soldCount < _amount) return;
			
		_isCompleted = true;
		TaskCompleted?.Invoke(this);
		EventBus.Instance.ItemSoldFromVendingMachine -= OnItemSold;
	}
}
