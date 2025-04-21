using System;
using System.Collections.Generic;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Quests;

public partial class QuestController : Node
{
	public event Action<Quest> QuestStarted;
	public event Action<QuestLine> QuestLineStarted;

	private QuestLine _currentQuestLine;
	private Quest _currentQuest;
	
	private static QuestController Instance { get; set; }
	
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		if (Instance != null)
		{
			_logger.Error("There are multiple instances of QuestController");
			QueueFree();
		}
		else
		{
			Instance = this;
		}

		if (EventBus.Instance == null)
		{
			throw new Exception("There is no instance of EventBus");
		}
		
		EventBus.Instance.OnQuestInitialised(this);
		_logger.Debug("Initialised.");
		var resource = GD.Load<QuestLine>("res://Resources/Quest/TutorialQuest.tres");
		StartQuestLine(resource);
	}
	
	private void StartQuestLine(QuestLine questLine)
	{
		_currentQuestLine = questLine;
		_currentQuest = _currentQuestLine.Quests[0];
		_logger.Debug("Quest line started: " + _currentQuestLine.Name);
		
		QuestLineStarted?.Invoke(_currentQuestLine);
		
		StartQuest(_currentQuest);
		
		// subscribe to quest progression for all quests in the quest line
		foreach (var quest in _currentQuestLine.Quests)
		{
			quest.QuestProgressionChanged += OnQuestProgressionChanged;
		}
	}

	private void OnQuestProgressionChanged(QuestProgression obj)
	{
		_logger.Debug("Quest progression changed: " + obj);
		//Check if the quest is completed
		if (obj == QuestProgression.Completed)
		{
			// Check if there are more quests in the quest line
			var currentQuestIndex = _currentQuestLine.Quests.IndexOf(_currentQuest);
			if (currentQuestIndex < _currentQuestLine.Quests.Count - 1)
			{
				_currentQuest = _currentQuestLine.Quests[currentQuestIndex + 1];
				StartQuest(_currentQuest);
			}
			else
			{
				// Quest line completed
				_logger.Debug("Quest line completed.");
			}
		}
		else
		{
			_logger.Debug("Quest progression changed: " + obj);
		}
	}

	private void StartQuest(Quest currentQuest)
	{
		_logger.Debug("Starting quest: " + currentQuest.Description);
		QuestStarted?.Invoke(currentQuest);
		currentQuest.InitialiseQuest();
	}
}
