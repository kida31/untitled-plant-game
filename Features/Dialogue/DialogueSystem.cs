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

	public void StartDialog(DialogueResourceObject dialogue)
	{
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
		switch (nextDialogue)
		{
			case null:
				_logger.Debug("No follow up dialogue");
				EndDialogue();
				return;
			case DialogueEvent d:
				d.Execute();
				EndDialogue();
				return;
			default:
				SetAndResetDialogueBlock(nextDialogue);
				break;
		}
	}

	public void GetResponses()
	{
		_logger.Debug("Getting responses.");
		if (_currentDialogue._responses.Length == 0)
		{
			EndDialogue();
			return;
		}

		OnResponding?.Invoke(_currentDialogue._responses.Select(r => r._responseButton).ToArray());
	}

	private void EndDialogue()
	{
		_logger.Debug("Ending dialogue.");
		_currentDialogue = null;
		GameStateMachine.Instance.SetState(GameState.FreeRoam);
		OnDialogueEnd?.Invoke(_currentDialogue);
	}

	private void SetAndResetDialogueBlock(DialogueResourceObject dialogue)
	{
		_currentDialogue = dialogue;
		OnDialogueBlockStarted?.Invoke(_currentDialogue);
	}
}
