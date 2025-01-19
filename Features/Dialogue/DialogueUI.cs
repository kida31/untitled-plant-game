using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue;

public partial class DialogueUI : Control
{
	private IDialogueSystem _dialogueSystem;

	[Export] private RichTextLabel _nameLabel;
	[Export] private RichTextLabel _dialogueTextLabel;
	[Export] private AnimatedSprite2D _animatedSprite2D;
	[Export] private BoxContainer _responseContainer;

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
		//Animation
		_skipCooldownTimer = new Timer();
		AddChild(_skipCooldownTimer);
		_skipCooldownTimer.Autostart = false;
		_skipCooldownTimer.OneShot = true;
		_dialogueAnimation = new DialogueAnimation();
		AddChild(_dialogueAnimation);

		//Events
		EventBus.Instance.OnNpcStartDialogue += ChangeToNpcPortrait;
		EventBus.Instance.InitialiseDialogue += ConnectDialogue;
		_logger.Debug("Subscribed to dialogue system intialising.");
		_skipCooldownTimer.Timeout += () => _smashable = true;
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_accept") && Visible)
		{
			OnPlayerInputConfirm();
		}
	}

	private void ConnectDialogue(IDialogueSystem sys)
	{
		if(_dialogueSystem != null)
		{
			_dialogueSystem.OnDialogueEnd -= HideDialogueUi;
			_dialogueSystem.OnDialogueBlockStarted -= OnDialogueBlockStarted;
			_dialogueSystem.OnResponding -= DisplayResponses;
			_dialogueSystem = null;
		}
		
		_logger.Debug("Dialogue system connected." + sys);
		_dialogueSystem = sys;
		_dialogueSystem.OnDialogueBlockStarted += OnDialogueBlockStarted;
		_dialogueSystem.OnDialogueEnd += HideDialogueUi;
		_dialogueSystem.OnResponding += DisplayResponses;
	}

	private void ChangeToNpcPortrait(AnimatedSprite2D portrait)
	{
		_animatedSprite2D.SpriteFrames = portrait.SpriteFrames;
		var r = _animatedSprite2D.SpriteFrames;
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
		if (_currentDialogue == null)
		{
			_logger.Warn("There is no dialogue to show."); //happens when player chooses a response TODO: ignore confirm response
			return;
		}

		if (!_smashable)
		{
			_logger.Debug("Stop smashing the button.");
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
			Button button = new Button();
			_responseContainer.CallDeferred(Node.MethodName.AddChild, button);
			button.Text = response;
			button.ActionMode = BaseButton.ActionModeEnum.Press;

			void OnButtonOnPressed()
			{
				_dialogueSystem.InsertSelectedResponse(response);
				ClearResponses();
			}

			button.Pressed += OnButtonOnPressed;
			buttons.Add(button);
		}

		buttons.First().CallDeferred(Control.MethodName.GrabFocus);
	}

	private void ClearResponses()
	{
		foreach (var child in _responseContainer.GetChildren())
		{ 
			_logger.Debug($"Queue {child} for deletion.");
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

	private void HideDialogueUi(DialogueResourceObject _)
	{
		_currentDialogue = null;
		_lineEnumerator = null;
		Visible = false;
	}
}
