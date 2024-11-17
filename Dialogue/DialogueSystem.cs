using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameState;

namespace untitledplantgame.Dialogue;

public partial class DialogueSystem : Node, IDialogueSystem
{
	private enum DialogueState
	{
		Conversing,
		Responding,
		End
	}

	public static DialogueSystem Instance { get; private set; }

	public event Action<DialogueResourceObject> OnDialogueStart;
	public event Action<DialogueResourceObject> OnDialogueEnd;

	private DialogueResourceObject _currentDialogue;
	private IEnumerator<DialogueLine> _enumerator;
	private DialogueState _state;
	private Logger _logger;
	private DialogueAnimation _dialogueAnimation;
	private Timer _timer;

	public override void _Ready()
	{
		_logger = new(this);
		
		if (Instance != null)
		{
			_logger.Error("There is already an instance of DialogueSystem.");
			QueueFree();
			return;
		}
		_dialogueAnimation = new DialogueAnimation();
		AddChild(_dialogueAnimation);
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
		if (_enumerator.MoveNext())
			DisplayLine(_enumerator.Current);
	}

	private void EndDialogue()
	{
		_currentDialogue = null;
		OnDialogueEnd?.Invoke(_currentDialogue);
		GameStateMachine.Instance.ChangeState(GameState.FreeRoam);
		_state = DialogueState.End;
	}

	private void SetAndResetDialogue(DialogueResourceObject dialogue)
	{
		_currentDialogue = dialogue;
		_enumerator = _currentDialogue._dialogueText.AsEnumerable().GetEnumerator();
		_enumerator.Reset();
		_state = DialogueState.Conversing;
	}

	/// <summary>
	/// Called whenever player presses the confirm button.
	/// </summary>
	private void OnPlayerInputConfirm()
	{
		if (_currentDialogue == null || _state == DialogueState.End)
		{
			return;
		}

		_logger.Debug("Player input confirm.");
		_logger.Debug("State: " + _state);
		
		if (_dialogueAnimation.IsPlaying)
		{
			_dialogueAnimation.SkipAnimation();
			return;
		}
		
		if (_enumerator.MoveNext())
		{
			DisplayLine(_enumerator.Current);
			if (_currentDialogue.responses.Count > 0 && _enumerator.Current == _currentDialogue._dialogueText.Last())
			{
				DisplayResponses();
			}

			return;
		}

		if (_state == DialogueState.Responding)
		{
			InsertSelectedResponse();
			return;
		}

		EndDialogue();
	}

	private void InsertSelectedResponse()
	{
		var currentSelection = _currentDialogue.responses.Keys.First(); //TODO: Implement selection

		var nextDialogue = _currentDialogue.responses[currentSelection];
		SetAndResetDialogue(nextDialogue);
		if (_enumerator.MoveNext())
			DisplayLine(_enumerator.Current);
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
		GD.Print($"{speaker} (${expr}):");
		_dialogueAnimation.SetLine(text);
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
