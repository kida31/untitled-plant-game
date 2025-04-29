using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants;

namespace untitledplantgame.Quests;

[GlobalClass]
public partial class WateringTask : QuestTask
{
	public override bool IsCompleted => _isCompleted;
	public override event Action<QuestTask> TaskCompleted;
	private bool _isCompleted = false;

	public override void StartTask()
	{
		EventBus.Instance.WateredSoil += OnWateredSoil;
	}

	protected override void StopTask()
	{
		EventBus.Instance.WateredSoil -= OnWateredSoil;
		TaskCompleted?.Invoke(this);
	}

	private void OnWateredSoil(SoilTile obj)
	{
		_isCompleted = true;

		StopTask();
	}
}
