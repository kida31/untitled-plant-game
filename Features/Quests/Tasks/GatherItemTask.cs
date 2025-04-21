using System;
using Godot;

namespace untitledplantgame.Quests;

public partial class GatherItemTask : QuestTask
{
	[Export] private string _itemId;
	[Export] private int _amount;
	
	public override bool IsCompleted => _isCompleted;
	public override event Action<QuestTask> TaskCompleted;
	private bool _isCompleted = false;
	
	public GatherItemTask()
	{
		
	}
}
