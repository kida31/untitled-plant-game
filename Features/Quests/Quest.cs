using System;
using Godot;

namespace untitledplantgame.Quests;

[GlobalClass]
public partial class Quest : Resource
{
	[Export] public string QuestId { get; set; }
	[Export] public string Description { get; set; }
	[Export] public QuestProgression Progression { get; set; }
	[Export] public QuestTask Task { get; set; }

	public event Action<QuestProgression> QuestProgressionChanged;
	public event Action<string> QuestCompleted;

	public Quest(string questId, string description, QuestProgression progression, QuestTask task)
	{
		QuestId = questId;
		Description = description;
		Progression = progression;
		Task = task;
		
		
	}

	private void OnQuestProgressionChanged(QuestProgression progression)
	{
		QuestProgressionChanged?.Invoke(progression);
	}

	protected virtual void OnQuestCompleted(string questId)
	{
		QuestCompleted?.Invoke(questId);
	}
}
