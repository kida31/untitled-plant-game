using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Quests;

[GlobalClass]
public partial class Quest : Resource
{
	[Export] public string Description { get; set; }
	[Export] public QuestTask Task { get; set; }
	
	public QuestProgression Progression { get; private set; }

	public event Action<QuestProgression> QuestProgressionChanged;

	private Logger _logger = new ("Quest");
	
	public Quest()
	{
		
	}

	public void InitialiseQuest()
	{
		Assert.AssertNotNull(Task, "Task is null. This Quest is invalid.");
		//TODO This should only be called once
		Task.TaskCompleted += OnTaskCompleted;
	}

	private void OnTaskCompleted(QuestTask obj)
	{
		Progression = QuestProgression.Completed;
		_logger.Debug("Quest is completed");
		
		OnQuestProgressionChanged(Progression);
	}

	private void OnQuestProgressionChanged(QuestProgression obj)
	{
		_logger.Debug("Quest progression changed: " + obj);
		QuestProgressionChanged?.Invoke(obj);
	}
}
