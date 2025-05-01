using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.NPC.NpcInteraction;
using untitledplantgame.Quests;

namespace untitledplantgame.NPC;

public partial class Godfrey : CharacterBody2D
{
	[Export] private AnimatedSprite2D _overWorldSprite;
	[Export] private NpcPlayerInteraction _npcPlayerInteraction;
	[Export] private DialogueResourceObject _introDialogue;
	[Export] private DialogueResourceObject _genericDialogue;

	private const string TutorialQuestLineId = "TutorialQuest";

	private DialogueResourceObject _currentDialogue;
	private Logger _logger = new("Godfrey");

	private bool _firstTimeSpokenTo = true;
	private bool _tutorialActive = false;

	public override void _Ready()
	{
		_npcPlayerInteraction.InteractionEvent += () =>
		{
			if (_tutorialActive)
			{
				var quest = QuestController.Instance.CurrentQuest;
				var questIndex = QuestController.Instance.CurrentQuestLine.Quests.IndexOf(quest);
				var dialogueTask = quest.Task as HaveDialogueTask;
				
				// _logger.Debug("Dialogue task is active. Setting dialogue to: " + _currentDialogue._dialogueId);
				_currentDialogue = questIndex switch
				{
					0 => LoadDialogue("WateringTaskInProgress"),
					2 => LoadDialogue("HarvestingTaskInProgress"),
					4 => LoadDialogue("SellingTaskInProgress"),
					5 => LoadDialogue("SellingTaskInProgress"),
					7 => LoadDialogue("TalkToPanDanTaskInProgress"),
					1 or 3 or 6 or 8 => dialogueTask!.Dialogue,
					_ => CreateOneLiner("Go do your thing."),
				};
				_logger.Debug($"Quest index is: {questIndex}. Setting dialogue to: {_currentDialogue._dialogueId}");
			}
			else if (_firstTimeSpokenTo)
			{
				_currentDialogue = _introDialogue;
				_firstTimeSpokenTo = false;
				_logger.Debug("First time spoken to. Setting dialogue to: " + _currentDialogue._dialogueId);
			}
			else
			{
				_currentDialogue = _genericDialogue;
				_logger.Debug("Generic dialogue. Setting dialogue to: " + _currentDialogue._dialogueId);
			}

			Assert.AssertNotNull(_currentDialogue, "Dialogue is null");
			EventBus.Instance.InvokeStartingDialogue(_currentDialogue);
		};

		QuestController.Instance.QuestLineCompleted += questLine =>
		{
			if (questLine.Id == TutorialQuestLineId)
			{
				_tutorialActive = false;
			}
		};
		QuestController.Instance.QuestLineStarted += questLine =>
		{
			if (questLine.Id == TutorialQuestLineId)
			{
				_tutorialActive = true;
			}
		};
	}

	private DialogueResourceObject CreateOneLiner(string sentence)
	{
		var dialogue = new DialogueResourceObject();
		dialogue._dialogueText =
		[
			new DialogueLine
			{
				dialogueText = sentence,
				DialogueExpression = null,
				speakerName = "Godfrey",
			}
		];
		dialogue._responses = null;
		return dialogue;
	}

	private DialogueResourceObject LoadDialogue(string file)
	{
		var filePath = $"res://Resources/Dialogue/Godfrey/Tutorial/TaskInProgress/{file}.tres";
		var dialogue = ResourceLoader.Load<DialogueResourceObject>(filePath);
		return dialogue;
	}
}
