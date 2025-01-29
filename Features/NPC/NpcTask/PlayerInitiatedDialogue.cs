using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Dialogue;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.NPC.NpcInteraction;
using untitledplantgame.NPC.RoutinePlanner;

namespace untitledplantgame.NPC.NpcTask;

// TODO: Cleanup
public partial class PlayerInitiatedDialogue : Node, ITaskInterruption
{
	[Export] private Array<DialogueResourceObject> _dialogueResourceObjects;
	[Export] private bool _randomOrderOfDialogueLines;

	private int _amountOfDialogueLinesUsed;
	private bool DialogueFinished { get; set; }
	private event EventHandler TaskStarted;
	private event EventHandler TaskFinished;
	private NpcRoutinePlanner _routinePlanner;
	private IDialogueSystem _dialogueSystem;
	private NpcPlayerInteraction _npcInteraction;
	
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);
		_routinePlanner = (NpcRoutinePlanner) GetParent(); // We will enforce this as a soft rule ⇒ RoutinePlanner MUST be the parent!
		
		_npcInteraction = (NpcPlayerInteraction) _routinePlanner.GetParent().FindChild("InteractionNode");
		_npcInteraction.InteractionEvent += StartDialogue;
	}
	
	private void ConnectDialogue(IDialogueSystem sys)
	{
		_dialogueSystem = sys;
		_dialogueSystem.OnDialogueEnd += FinishDialogue;
	}
	
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
	
		TaskStarted?.Invoke(this, EventArgs.Empty);
		
		
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
		TaskFinished?.Invoke(this, EventArgs.Empty);
		
		_routinePlanner.LastRoutine?.ContinueRoutine();
		_routinePlanner.ActiveTask?.ResumeCurrentTask();
		_logger.Info("The Npc is now resuming it's original task.");
	}

	public async void ResumeRoutineIfFinished()
	{
		await Task.Yield();
		
		await WaitForConditionAsync();
	}
	
	
	private Task WaitForConditionAsync()
	{
		var tcs = new TaskCompletionSource<bool>();
		
		EventHandler onConditionMet = null;
		onConditionMet = (_, _) =>
		{
			if (!DialogueFinished)
			{
				return;
			}
			
			tcs.TrySetResult(true);
			TaskFinished -= onConditionMet;
		};
		TaskFinished += onConditionMet;
		
		return tcs.Task;
	}
}
