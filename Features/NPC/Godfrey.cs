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
	private bool _tutorialCompleted = false;
	
	public override void _Ready()
	{
		_currentDialogue = _introDialogue;
		QuestController.Instance.QuestStarted += quest =>
		{
			if(quest.Task is HaveDialogueTask dialogueTask)
			{
				_currentDialogue = dialogueTask.Dialogue;
			}
		};
		QuestController.Instance.QuestLineCompleted += questLine =>
		{
			if(questLine.Id == TutorialQuestLineId)
			{
				_tutorialCompleted = true;
			}
		};
		_npcPlayerInteraction.InteractionEvent += () =>
		{
			if (_firstTimeSpokenTo)
			{
				_firstTimeSpokenTo = false;
			}
			if (!_firstTimeSpokenTo)
			{
				_currentDialogue = _genericDialogue;
			}
			
			
			EventBus.Instance.InvokeStartingDialogue(_currentDialogue);
		};
	}
}
