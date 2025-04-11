using Godot;
using System;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Dialogue;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.NPC.NpcInteraction;
using untitledplantgame.NPC.RoutinePlanner;

namespace untitledplantgame.NPC.NpcTask;

/// <summary>
///		The PlayerInitiatedDialogue class is similar to the "TalkToPlayerTask", but is supposed to be used as a TaskInterruption.
///		The key difference between the two is the Npc will wait for the player to initiate the "TalkToPlayerTask", meaning it will wait
///		until the Player approaches the Npc before continuing with the rest of the Tasks in the given Routine. 
/// </summary>
public partial class PlayerInitiatedDialogue : Node, ITaskInterruption
{
	[Export] private Array<DialogueResourceObject> _dialogueResourceObjects;
	[Export] private bool _randomOrderOfDialogueLines;

	private int _amountOfDialogueLinesUsed;
	private bool DialogueFinished { get; set; }
	private NpcRoutinePlanner _routinePlanner;
	private IDialogueSystem _dialogueSystem;
	private NpcPlayerInteraction _npcInteraction;
	
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		_routinePlanner = GetRoutinePlanner(); // We will enforce this as a soft rule ⇒ RoutinePlanner MUST be the parent!
		
		_npcInteraction = (NpcPlayerInteraction) _routinePlanner.GetParent().FindChild("InteractionNode");
		_npcInteraction.InteractionEvent += StartDialogue;
	}

	public NpcRoutinePlanner GetRoutinePlanner()
	{
		return (NpcRoutinePlanner) GetParent();
	}
	
	private void ConnectDialogue(IDialogueSystem sys)
	{
		_dialogueSystem = sys;
		_dialogueSystem.OnDialogueEnd += FinishDialogue;
	}
	
	// Essentially only differentiates between "a Task is active" and "no Task is active".
	private void StartDialogue()
	{
		EventBus.Instance.InitialiseDialogue += ConnectDialogue;

		if (_randomOrderOfDialogueLines)
		{
			EventBus.Instance.InvokeStartingDialogue(_dialogueResourceObjects[new Random().Next(_dialogueResourceObjects.Count)]);
		}
		else
		{
			EventBus.Instance.InvokeStartingDialogue(_dialogueResourceObjects[_amountOfDialogueLinesUsed]);
			_amountOfDialogueLinesUsed++;
			
			if (_amountOfDialogueLinesUsed >= _dialogueResourceObjects.Count)
			{
				_amountOfDialogueLinesUsed = 0;
			}
		}
		
		
		if (_routinePlanner.ActiveTask != null)
		{
			_routinePlanner.ActiveTask?.InterruptCurrentTask();
		}
		else
		{
			if (_routinePlanner.LastRoutine != null)
			{
				_routinePlanner.LastRoutine?.NextRoutine.InterruptRoutine();
			}
			else
			{
				_routinePlanner.StartingRoutine.InterruptRoutine(); // The first routine gets instantly set as the last routine
			}
		}
		
		_logger.Info("Player stopped the current routine by starting a Dialogue with an Npc.");
	}
	
	private void FinishDialogue(DialogueResourceObject _)
	{
		EventBus.Instance.InitialiseDialogue -= ConnectDialogue;
		_dialogueSystem.OnDialogueEnd -= FinishDialogue;
		
		DialogueFinished = true;
		
		_routinePlanner.LastRoutine?.ContinueRoutine();
		_routinePlanner.ActiveTask?.ResumeCurrentTask();
		_logger.Info("The Npc is now resuming it's original task.");
	}
}
