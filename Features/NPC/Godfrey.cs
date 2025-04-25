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
	
	[Export] private DialogueResourceObject[] _questDialogues;

	private bool _firstTimeSpokenTo = true;
	private bool _tutorialCompleted = false;
	
	public override void _Ready()
	{
		QuestController.Instance.QuestStarted += quest =>
		{
			
		};
		_npcPlayerInteraction.InteractionEvent += () =>
		{
			if (_firstTimeSpokenTo)
			{
				EventBus.Instance.InvokeStartingDialogue(_introDialogue);
				_firstTimeSpokenTo = false;
			}
			else if (_tutorialCompleted)
			{
				EventBus.Instance.InvokeStartingDialogue(_introDialogue);
			}
			
		};
	}
}
