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

	private const string TutorialQuestLineId = "tutorial";

	private DialogueResourceObject _currentDialogue;

	private bool _firstTimeSpokenTo = true;
	private bool _tutorialActive = false;

	public override void _Ready()
	{
		_npcPlayerInteraction.InteractionEvent += () =>
		{
			if (_tutorialActive)
			{
				var quest = QuestController.Instance.CurrentQuest;
				if (quest.Task is HaveDialogueTask dialogueTask)
				{
					_currentDialogue = dialogueTask.Dialogue;
				}
				else
				{
					var questIndex = QuestController.Instance.CurrentQuestLine.Quests.IndexOf(quest);

					_currentDialogue = questIndex switch
					{
						0 => LoadDialogue("WateringTaskInProgress", "DE"),
						2 => LoadDialogue("HarvestingTaskInProgress", "DE"),
						4 => LoadDialogue("SellingTaskInProgress", "DE"),
						5 => LoadDialogue("SellingTaskInProgress", "DE"),
						_ => CreateOneLiner("Go do your thing."),
					};
				}
			}
			else if (_firstTimeSpokenTo)
			{
				_currentDialogue = _introDialogue;
				_firstTimeSpokenTo = false;
			}
			else
			{
				_currentDialogue = _genericDialogue;
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

	private DialogueResourceObject LoadDialogue(string file, string language)
	{
		var filePath = $"res://Resources/Dialogue/Godfrey/Tutorial/TaskInProgress/{file}_{language}.tres";
		if (!FileAccess.FileExists(filePath))
		{
			filePath = $"res://Resources/Dialogue/Godfrey/Tutorial/TaskInProgress/{file}.tres";
		}

		var dialogue = ResourceLoader.Load<DialogueResourceObject>(filePath);
		return dialogue;
	}
}
