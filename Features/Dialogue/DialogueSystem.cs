using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue;

public partial class DialogueSystem : Node, IDialogueSystem
{
	public event Action<DialogueResourceObject> OnDialogueStart;
	public event Action<DialogueResourceObject> OnDialogueEnd;

	private DialogueResourceObject _currentDialogue;
	private IEnumerator<DialogueLine> _enumerator;
	private DialogueState _state;
	private Logger _logger;
	private DialogueUi _dialogueUi;
	private Timer _skipCooldownTimer;
	private int _waitForSeconds = 1;
	private bool _smashable = true;

	public override void _Ready()
	{
		_logger = new(this);

		_skipCooldownTimer = new Timer();
		AddChild(_skipCooldownTimer);
		_skipCooldownTimer.Autostart = false;
		_skipCooldownTimer.OneShot = true;
		_skipCooldownTimer.Timeout += () => _smashable = true;
		_dialogueUi = GetNode<DialogueUi>("DialogueUi");
		_logger.Debug(_dialogueUi.Name);
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_accept"))
		{
			OnPlayerInputConfirm();
		}
	}

	public void StartDialog(string dialogueId)
	{
		var dialogue = DialogueDatabase.Instance.GetResourceByName(dialogueId);

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
		OnDialogueStart?.Invoke(dialogue);

		SetAndResetDialogue(dialogue);
		if (_enumerator.MoveNext())
			DisplayLine(_enumerator.Current);
	}

	private void EndDialogue()
	{
		_currentDialogue = null;
		GameStateMachine.Instance.SetState(GameState.FreeRoam);
		_state = DialogueState.End;
		_dialogueUi.ClearDialogue();
		OnDialogueEnd?.Invoke(_currentDialogue);
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
		if (!_smashable)
		{
			return;
		}

		if (_currentDialogue == null || _state != DialogueState.Conversing)
		{
			return;
		}

		_logger.Debug("Player input confirm.");
		_logger.Debug("State: " + _state);

		if (_dialogueUi.AnimationIsPlaying)
		{
			_smashable = false;
			_dialogueUi.ShowAllDialogue();
			_skipCooldownTimer.Start(_waitForSeconds);
			return;
		}

		if (_enumerator.MoveNext())
		{
			DisplayLine(_enumerator.Current);
			if (_currentDialogue._responses.Length > 0 && _enumerator.Current == _currentDialogue._dialogueText.Last())
			{
				DisplayResponses();
			}

			return;
		}

		EndDialogue();
	}

	public void InsertSelectedResponse(string response)
	{
		var nextDialogue = _currentDialogue._responses.First((r) => r._responseButton == response)._responseDialogue;
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

		var speaker = line.speakerName;
		var expr = line.DialogueExpression.ToString();
		var text = line.dialogueText;
		_logger.Debug($"{speaker} (${expr}):${text}");
		_dialogueUi.DisplayDialogue(line);
	}

	private void DisplayResponses()
	{
		// Display responses
		foreach (var response in _currentDialogue._responses)
		{
			_logger.Debug("- " + response);
		}

		_dialogueUi.DisplayResponses(_currentDialogue._responses.Select(r => r._responseButton).ToArray());

		_state = DialogueState.Responding;
	}
	
	private enum DialogueState
	{
		Conversing,
		Responding,
		End,
	}
}
