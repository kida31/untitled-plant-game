using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Dialogue;
using untitledplantgame.Dialogue.Models;
using untitledplantgame.NPC.NpcInteraction;
using Array = Godot.Collections.Array;

namespace untitledplantgame.NPC.NpcTask;

/**
 * This task is used to force the NPC to chat with the Player.
 * The NPC waits until the Player approaches him and initiates a conversation. The Player then has to finish the conversation.
 *
 * The task is finished when the Player has finished the conversation and Dialogue System invokes "OnDialogueEnd"
 */
public partial class TalkToPlayerTask :  Node, INpcTask
{
	[Export] private DialogueResourceObject _dialogueResourceObject;

	private Array<ResponseAction> _responseActionsList;
	
	private bool DialogueFinished { get; set; }
	private int _dialogueIndex;
	private event EventHandler TaskStarted;
	private event EventHandler TaskFinished;
	private Npc _npcExecutingThisTasks;
	private NpcPlayerInteraction _npcInteraction;
	private IDialogueSystem _dialogueSystem;
	
	private Logger _logger;

	public override void _Ready()
	{ 
		_responseActionsList = new ();
		
		foreach (var childNode in GetChildren())
		{
			if (childNode is ResponseAction node)
			{
				_responseActionsList.Add(node);
			}
			else
			{
				_logger.Warn("The node: " + childNode.Name + " is not of type 'ResponseAction'! It will be ignored.");
			}
			
		}
		
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
		EventBus.Instance.OnResponseButtonPress += TriggerActionAfterResponse;
		EventBus.Instance.InvokeStartingDialogue(_dialogueResourceObject);
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

	/*
	 * I lost the plot somewhere along the lines of "PlayerInteraction" and the fifth event...
	 *
	 * The method gets unsubscribed first, then the event gets invoked, which doesn't make sense for obvious reasons.
	 * Waiting just a single 1/1000 of a second forces the game to execute this code at a later point, resulting in desired behavior
	 */
	private async void DelayUnsubscribe()
	{
		await Task.Yield();
		EventBus.Instance.OnResponseButtonPress -= TriggerActionAfterResponse;
		await Task.Delay(1);
	}

	public bool IsTaskActive()
	{
		return DialogueFinished;
	}

	public void InterruptCurrentTask()
	{
		_logger.Error("TalkToPlayerTask can't be interrupted! The player has to finish the dialogue first! Something went wrong.");
	}

	public void ResumeCurrentTask()
	{
		_logger.Error("TalkToPlayerTask can't be resumed, since it can't be interrupted!");
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

	private void TriggerActionAfterResponse(string responseText)
	{
		var lastResponses = GetDeepestResponses(_dialogueResourceObject);
		var index = 0;
		
		foreach (var responseObjects in lastResponses)
		{
			var currentResponseButton = responseObjects._responseButton;

			if (index > _responseActionsList.Count-1)
			{
				return;
			}
			
			if (responseText == currentResponseButton)
			{
				_responseActionsList[index].ActionAfterResponse();
				return;
			}

			index++;
			
		}
	}

	private DialogueResponse[] GetDeepestResponses(DialogueResourceObject dialogueResourceObject)
	{
		int maxDepth = 0;
		List<DialogueResponse> deepestList = new List<DialogueResponse>();

		void Traverse(DialogueResponse response, int depth)
		{
			// If _responseDialogue is null, treat this response as the last one in its branch
			if (response._responseDialogue == null)
			{
				if (depth > maxDepth)
				{
					maxDepth = depth;
					deepestList.Clear();
					deepestList.Add(response);
				}
				else if (depth == maxDepth)
				{
					deepestList.Add(response);
				}
				return;
			}

			// If _responseDialogue._responses is null or empty, it's also a leaf
			var subResponses = response._responseDialogue._responses;
			if (subResponses == null || subResponses.Length == 0)
			{
				if (depth > maxDepth)
				{
					maxDepth = depth;
					deepestList.Clear();
					deepestList.Add(response);
				}
				else if (depth == maxDepth)
				{
					deepestList.Add(response);
				}
				return;
			}

			// Recursive case: traverse into sub-responses
			foreach (var subResponse in subResponses)
			{
				Traverse(subResponse, depth + 1);
			}
		}

		// Start recursion for each top-level response
		foreach (var response in dialogueResourceObject._responses)
		{
			Traverse(response, 1);
		}

		return deepestList.ToArray();
	}
}
