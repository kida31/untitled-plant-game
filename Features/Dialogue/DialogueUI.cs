using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue;

public partial class DialogueUi : Control
{
	[Export] private DialogueSystem _dialogueSystem;
	
	private RichTextLabel _nameLabel;
	private RichTextLabel _dialogueTextLabel;
	private AnimatedSprite2D _animatedSprite2D;
	private BoxContainer _responseContainer;
	
	
	private DialogueResourceObject _currentDialogue;
	private IEnumerator<DialogueLine> _lineEnumerator;
	
	private DialogueAnimation _dialogueAnimation;
	private bool _smashable = true;
	private double _waitForSeconds = 0.5;
	private Timer _skipCooldownTimer;
	
	private Logger _logger;
	private bool AnimationIsPlaying => _dialogueAnimation.AnimationIsPlaying;

	public override void _Ready()
	{
		_logger = new Logger(this);
		
		//UI elements
		_animatedSprite2D = GetNode<AnimatedSprite2D>("Portrait");
		_nameLabel = GetNode<RichTextLabel>("PanelContainer2/MarginContainer/Name");
		_dialogueTextLabel = GetNode<RichTextLabel>("PanelContainer/MarginContainer/DialogueText");
		_responseContainer = GetNode<BoxContainer>("Responses");
		
		//Animation
		_skipCooldownTimer = new Timer();
		AddChild(_skipCooldownTimer);
		_skipCooldownTimer.Autostart = false;
		_skipCooldownTimer.OneShot = true;
		_dialogueAnimation = new DialogueAnimation();
		AddChild(_dialogueAnimation);
		
		//Events
		_dialogueSystem.OnDialogueBlockStarted += OnDialogueBlockStarted;
		_dialogueSystem.OnDialogueEnd += o => OnEndOfDialogueBlock();
		_dialogueSystem.OnResponding += DisplayResponses;
		_skipCooldownTimer.Timeout += () => _smashable = true;
	}
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_accept"))
		{
			OnPlayerInputConfirm();
		}
	}
	private void OnDialogueBlockStarted(DialogueResourceObject dialogue)
	{
		_currentDialogue = dialogue;
		_lineEnumerator = dialogue._dialogueText.AsEnumerable().GetEnumerator();
		_lineEnumerator.MoveNext(); //Enumerator starts at index -1
		ShowDialogueLine(_lineEnumerator.Current);
	}
	
	private void OnPlayerInputConfirm()
	{
		if (!_smashable || _currentDialogue == null)
		{
			_logger.Warn("Stop smashing the button.");
			return;
		}

		_logger.Debug("Player input confirm.");

		if (AnimationIsPlaying)
		{
			_logger.Debug("Skipping animation.");
			SkipAnimation();
			return;
		}

		_smashable = true;
		if (_lineEnumerator.MoveNext()) //End of Line
		{
			_logger.Debug("Showing next line.");
			ShowDialogueLine(_lineEnumerator.Current);
			return;
		}
		
		_logger.Debug("End of dialogue block.");
		OnEndOfDialogueBlock();
	}

	//Displays dialogue on the screen
	private void ShowDialogueLine(DialogueLine line)
	{
		_nameLabel.Text = line.speakerName;
		_dialogueTextLabel.Text = line.dialogueText;
		_dialogueAnimation.AnimateNextDialogueLine(_dialogueTextLabel, line);
		_animatedSprite2D.Play(line.DialogueExpression.ToString());
		Visible = true;
	}

	private void DisplayResponses(string[] responses)
	{
		var buttons = new List<Button>();
		foreach (var response in responses)
		{
			Button button;
			_responseContainer.CallDeferred(Node.MethodName.AddChild, button = new Button());
			button.Text = response;
			button.ActionMode = BaseButton.ActionModeEnum.Press;
			button.Pressed += () =>
			{
				_dialogueSystem.InsertSelectedResponse(response);
				ClearResponses();
			};
			buttons.Add(button);
		}

		buttons.First().CallDeferred(Control.MethodName.GrabFocus);
	}

	private void ClearResponses()
	{
		foreach (Node child in _responseContainer.GetChildren())
		{
			child.QueueFree();
		}
	}
	
	private void OnEndOfDialogueBlock()
	{
		_currentDialogue = null;
		_dialogueSystem.GetResponses();
	}

	private void SkipAnimation()
	{
		_dialogueAnimation.StopAnimation();
		_smashable = false;
		_skipCooldownTimer.Start(_waitForSeconds);
	}
	
	private void ShowDialogueUi(DialogueResourceObject dialogue)
	{
		OnDialogueBlockStarted(dialogue);
		Visible = true;
	}
	
	private void HideDialogueUi()
	{
		_currentDialogue = null;
		_lineEnumerator = null;
		Visible = false;
	}
}
