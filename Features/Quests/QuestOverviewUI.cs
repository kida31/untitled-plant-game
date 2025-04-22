using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Quests;

public partial class QuestOverviewUI : Control
{
	[Export] private Label QuestTitleLabel;
	[Export] private RichTextLabel QuestTaskLabel;
	
	private QuestController _questController;
	private readonly Logger _logger = new ("QuestOverviewUI");
	private QuestLine _currentQuestLine;

	public override void _Ready()
	{
		EventBus.Instance.InitialiseQuest += ConnectQuestUi;
	}

	private void ConnectQuestUi(QuestController questController)
	{
		_questController = questController;
		_questController.QuestStarted += UpdateQuestTaskLabel;
		_questController.QuestLineStarted += UpdateQuestTitleLabel;
		_questController.QuestLineCompleted += OnQuestLineCompleted;
		UpdateQuestTitleLabel(questController.CurrentQuestLine);
		UpdateQuestTaskLabel(questController.CurrentQuest);
	}

	private void OnQuestLineCompleted(QuestLine obj)
	{
		_logger.Debug("Quest line completed.");
		Visible = false;
	}

	private void UpdateQuestTaskLabel(Quest obj)
	{
		_logger.Debug("New Quest started: " + obj.Description);
		QuestTaskLabel.Text = obj.Description;
	}
	
	private void UpdateQuestTitleLabel(QuestLine obj)
	{
		_logger.Debug("New Quest line started: " + obj.Name);
		Visible = true;
		QuestTitleLabel.Text = obj.Name;
	}
}
