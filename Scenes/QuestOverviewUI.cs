using Godot;
using System;
using untitledplantgame.Common;
using untitledplantgame.Quests;

public partial class QuestOverviewUI : Control
{
	[Export] private Label QuestTitleLabel;
	[Export] private RichTextLabel QuestTaskLabel;
	
	private QuestController _questController;
	
	public QuestOverviewUI()
	{
		EventBus.Instance.InitialiseQuest += ConnectQuestUi;
	}

	private void ConnectQuestUi(QuestController questController)
	{
		_questController = questController;
		_questController.QuestStarted += UpdateQuestTaskLabel;
		_questController.QuestLineStarted += UpdateQuestTitleLabel;
	}

	private void UpdateQuestTaskLabel(Quest obj)
	{
		QuestTaskLabel.Text = obj.Description;
	}
	
	private void UpdateQuestTitleLabel(QuestLine obj)
	{
		QuestTitleLabel.Text = obj.Name;
	}
}
