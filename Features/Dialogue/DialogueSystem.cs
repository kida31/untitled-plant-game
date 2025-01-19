using System;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue;

public partial class DialogueSystem : Node, IDialogueSystem
{
	public event Action<DialogueResourceObject> OnDialogueEnd;
	public event Action<string[]> OnResponding;
	public event Action<DialogueResourceObject> OnDialogueBlockStarted;

	private static DialogueSystem Instance { get; set; }

	private DialogueResourceObject _currentDialogue;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new(this);

		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			_logger.Error("There are multiple instances of DialogueSystem");
			QueueFree();
		}


		EventBus.Instance.StartingDialogue += StartDialog;
		_logger.Debug("Initialised.");
	}
	

	public void StartDialog(string dialogueId)
	{
		var dialogue = DialogueDatabase.Instance.GetResourceByName(dialogueId);
		EventBus.Instance.InvokeInitialiseDialogue(this);
		
		if (dialogue == null)
		{
			_logger.Error("Dialogue is null.");
			return;
		}

		if (dialogue._dialogueText.Length == 0)
		{
			_logger.Error("Dialogue has no text.");
			return;
		}

		GameStateMachine.Instance.SetState(GameState.Dialogue);
		SetAndResetDialogueBlock(dialogue);
	}

	public void InsertSelectedResponse(string response)
	{
		var nextDialogue = _currentDialogue._responses.First((r) => r._responseButton == response)._responseDialogue;
		SetAndResetDialogueBlock(nextDialogue);
	}

	public void GetResponses()
	{
		if (_currentDialogue._responses.Length == 0)
		{
			EndDialogue();
			return;
		}
		OnResponding?.Invoke(_currentDialogue._responses.Select(r => r._responseButton).ToArray());
	}

	private void EndDialogue()
	{
		_currentDialogue = null;
		GameStateMachine.Instance.SetState(GameState.FreeRoam);
		OnDialogueEnd?.Invoke(_currentDialogue);
	}

	private void SetAndResetDialogueBlock(DialogueResourceObject dialogue)
	{
		OnDialogueBlockStarted?.Invoke(dialogue);
		_currentDialogue = dialogue;
	}
}
