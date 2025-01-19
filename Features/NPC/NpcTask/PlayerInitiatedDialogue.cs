using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Dialogue;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.NPC.NpcInteraction;
using untitledplantgame.NPC.RoutinePlanner;

namespace untitledplantgame.NPC.NpcTask;

public partial class PlayerInitiatedDialogue : Node, ITaskInterruption
{
	[Export] private Array<DialogueResourceObject> _dialogueResourceObject;
	
	private bool DialogueFinished { get; set; }
	private event EventHandler TaskStarted;
	private event EventHandler TaskFinished;
	private NpcRoutinePlanner _routinePlanner;
	private IDialogueSystem _dialogueSystem;
	private NpcPlayerInteraction _npcInteraction;

	public override void _Ready()
	{
		_routinePlanner = (NpcRoutinePlanner) GetParent(); // We will enforce this as a soft rule ⇒ RoutinePlanner MUST be the parent!
		
		_npcInteraction = (NpcPlayerInteraction) _routinePlanner.GetParent().FindChild("InteractionNode");
		_npcInteraction.InteractionEvent += StartDialogue;
	}
	
	private void ConnectDialogue(IDialogueSystem sys)
	{
		_dialogueSystem = sys;
		_dialogueSystem.OnDialogueEnd += o => { FinishDialogue(); };
	}
	
	private void StartDialogue()
	{
		EventBus.Instance.InitialiseDialogue += ConnectDialogue;
		EventBus.Instance.InvokeStartingDialogue(
			_dialogueResourceObject[new Random().Next(_dialogueResourceObject.Count)]._dialogueId);
		TaskStarted?.Invoke(this, EventArgs.Empty);
		
		_routinePlanner.ActiveTask?.InterruptCurrentTask();
		
		ResumeRoutineIfFinished();
	}
	
	private void FinishDialogue()
	{
		EventBus.Instance.InitialiseDialogue -= ConnectDialogue;
		
		DialogueFinished = true;
		TaskFinished?.Invoke(this, EventArgs.Empty);
		
		_routinePlanner.ActiveTask?.ResumeCurrentTask();
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
