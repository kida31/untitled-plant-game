using System;
using System.Net;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameState;

namespace untitledplantgame.Dialogue;

public partial class DialogueSystem : Node, IDialogueSystem
{
	public static DialogueSystem Instance { get; private set; }
	
	public event Action<DialogueResourceObject> OnDialogueStart;
	public event Action<DialogueResourceObject> OnDialogueEnd;

	private DialogueResourceObject _currentDialogue;
	private int _currentLineIndex;
	private Logger _logger;
	public override void _Ready()
	{
		_logger = new(this);
		
		if (Instance != null)
		{
			_logger.Error("There is already an instance of DialogueSystem.");
			QueueFree();
			return;
		}
		
		Instance = this;
	}

	public void StartDialog(DialogueResourceObject dialogue)
	{
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
		
		GameStateMachine.Instance.ChangeState(GameState.Dialogue);
		OnDialogueStart?.Invoke(dialogue);

		_currentLineIndex = 0;
		GD.Print(dialogue._dialogueText[_currentLineIndex]);
	}

	/// <summary>
	/// Called whenever player presses the confirm button.
	/// </summary>
	private void OnPlayerInputConfirm()
	{
		_currentLineIndex += 1;
		
		if (_currentLineIndex >= _currentDialogue._dialogueText.Length)
		{
			// Dont end yet
			OnDialogueEnd?.Invoke(_currentDialogue);
			GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
		}
		else
		{
			GD.Print(_currentDialogue?._dialogueText[_currentLineIndex]);	
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_confirm"))
		{
			OnPlayerInputConfirm();
		}
	}
}
