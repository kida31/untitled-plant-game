using System;
using System.Linq;
using System.Net;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameState;

namespace untitledplantgame.Dialogue;
// TODO:
// - Show responses directly after last line ends, not on input

public partial class DialogueSystem : Node, IDialogueSystem
{
	enum DialogueState
	{
		Conversing,
		Responding,
	}

	public static DialogueSystem Instance { get; private set; }

	public event Action<DialogueResourceObject> OnDialogueStart;
	public event Action<DialogueResourceObject> OnDialogueEnd;

	private DialogueResourceObject _currentDialogue;
	private int _currentLineIndex;
	private DialogueState _state;
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

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_accept"))
		{
			OnPlayerInputConfirm();
		}
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

		SetAndResetDialogue(dialogue);
		DisplayCurentLine();
	}

	private void SetAndResetDialogue(DialogueResourceObject dialogue)
	{
		_currentDialogue = dialogue;
		_currentLineIndex = 0;
		_state = DialogueState.Conversing;
	}

	/// <summary>
	/// Called whenever player presses the confirm button.
	/// </summary>
	private void OnPlayerInputConfirm()
	{
		if (_currentDialogue == null)
		{
			return;
		}

		_logger.Debug("Player input confirm.");

		_currentLineIndex += 1;

		// If dialogue has more lines
		if (_currentLineIndex < _currentDialogue._dialogueText.Length)
		{
			DisplayCurentLine();
			return;
		}

		// No more dialogue lines
		if (_state == DialogueState.Responding)
		{
			InsertSelectedResponse();
			return;
		}

		if (!_currentDialogue.responses.Any())
		{
			_currentDialogue = null;
			OnDialogueEnd?.Invoke(_currentDialogue);
			GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
			return;
		}

		// Display responses
		DisplayResponses();
	}

	private void InsertSelectedResponse()
	{
		var currentSelection = _currentDialogue.responses.Keys.First();
		// "Yes"

		var nextDialogue = _currentDialogue.responses[currentSelection];
		SetAndResetDialogue(nextDialogue);
		DisplayCurentLine();
	}

	private void DisplayLine(DialogueLine line)
	{
		if (line == null)
		{
			_logger.Error("Dialogue line is null.");
			return;
		}

		// Display dialogue line
		var speaker = line.speakerName;
		var expr = line.DialogueExpression.ToString();
		var text = line.dialogueText;
		GD.Print($"{speaker} (${expr}): {text}");
	}

	private void DisplayCurentLine()
	{
		DisplayLine(_currentDialogue?._dialogueText[_currentLineIndex]);
	}

	private void DisplayResponses()
	{
		// Display responses
		foreach (var response in _currentDialogue.responses.Keys)
		{
			GD.Print("- " + response);
		}

		_state = DialogueState.Responding;
	}
}
