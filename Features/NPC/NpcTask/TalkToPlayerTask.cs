using System;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Dialogue;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.NPC.NpcInteraction;

namespace untitledplantgame.NPC.NpcTask;

/// <summary>
///		This task is used to force the NPC to chat with the Player.
///		The NPC waits until the Player approaches him and initiates a conversation. The Player then has to finish the conversation.
///
///		The task is finished when the Player has finished the conversation and Dialogue System invokes "OnDialogueEnd".
/// </summary>
public partial class TalkToPlayerTask :  Node, INpcTask
{
	[Export] private Array<DialogueResourceObject> _dialogueResourceObjects;
	[Export] private bool _randomOrderOfDialogueLines;
	
	private bool DialogueFinished { get; set; }
	private int _amountOfDialogueLinesUsed;
	private event EventHandler TaskStarted;
	private event EventHandler TaskFinished;
	private Npc _npcExecutingThisTasks;
	private NpcPlayerInteraction _npcInteraction;
	private IDialogueSystem _dialogueSystem;
	
	private Logger _logger;

	public override void _Ready()
	{ 
		base._Ready();
		_logger = new Logger(this);
	}

	public void InitializeTask(Npc owningNpc)
	{
		_npcExecutingThisTasks = owningNpc;
		_npcInteraction = (NpcPlayerInteraction) _npcExecutingThisTasks.FindChild("InteractionNode");
		_npcInteraction.InteractionEvent += StartTask;
		_logger.Debug("Task assigned " + _npcExecutingThisTasks.GetNpcName() + " as it's owner.");
	}

	public void StartTask()
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
			
			if (_amountOfDialogueLinesUsed == _dialogueResourceObjects.Count)
			{
				_amountOfDialogueLinesUsed = 0;
			}
		}
		TaskStarted?.Invoke(this, EventArgs.Empty);
		_logger.Info("TalkToPlayerTask started.");
	}

	public void FinishTask()
	{
		_npcInteraction.InteractionEvent -= StartTask;
		EventBus.Instance.InitialiseDialogue -= ConnectDialogue;
		DelayUnsubscribe();
		
		DialogueFinished = true;
		TaskFinished?.Invoke(this, EventArgs.Empty);
		_logger.Info("TalkToPlayerTask finished.");
	}
	
	private async void DelayUnsubscribe()
	{
		await Task.Yield();
		await Task.Delay(1);
	}
	
	public void InterruptCurrentTask()
	{
		_logger.Error("TalkToPlayerTask can't be interrupted! The player has to finish the dialogue first! Something went wrong.");
	}

	public void ResumeCurrentTask()
	{
		_logger.Error("TalkToPlayerTask can't be resumed, since there isn't supposed to be an active Task in the first place!");
	}
	
	public async Task ExecuteNpcTask()
	{
		_logger.Debug("Async Task execution started.");
		await Task.Yield();
		StartTask();
		
		await WaitForConditionAsync();
	}
	
	private void ConnectDialogue(IDialogueSystem sys)
	{
		_dialogueSystem = sys;
		_dialogueSystem.OnDialogueEnd += o => { FinishTask(); };
	}
	
	/// <summary>
	///		Uses an EventHandler to check if a Npc has finished the Dialogue.
	/// </summary>
	/// <returns></returns>
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
			_logger.Debug("Dialogue is finished! Async Condition: 'DialogueFinish' is true.");
			
			tcs.TrySetResult(true);
			TaskFinished -= onConditionMet;
		};
		TaskFinished += onConditionMet;
		
		return tcs.Task;
	}
}
