using System.Collections.Generic;
using System.Linq;
using Godot;
using DialogueAnimation = untitledplantgame.Features.Dialogue.DialogueAnimation;
using DialogueLine = untitledplantgame.Features.Dialogue.Models.DialogueLine;
using DialogueSystem = untitledplantgame.Features.Dialogue.DialogueSystem;

public partial class DialogueCanvas : CanvasLayer
{
	private RichTextLabel _nameLabel;
	private RichTextLabel _dialogueTextLabel;
	private CanvasLayer _dialogueCanvas;
	private AnimatedSprite2D _animatedSprite2D;
	private BoxContainer _responseContainer;
	private DialogueAnimation _dialogueAnimation;

	private DialogueSystem _dialogueSystem;
	private int _currentDialogueIndex;

	public bool AnimationIsPlaying => _dialogueAnimation.AnimationIsPlaying;

	public override void _Ready()
	{
		_dialogueCanvas = GetNode<CanvasLayer>(".");
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_nameLabel = GetNode<RichTextLabel>("PanelContainer2/MarginContainer/Name");
		_dialogueTextLabel = GetNode<RichTextLabel>("PanelContainer/MarginContainer/DialogueText");
		_responseContainer = GetNode<BoxContainer>("Responses");
		_dialogueSystem = DialogueSystem.Instance;

		_dialogueAnimation = new DialogueAnimation();
		AddChild(_dialogueAnimation);
	}

	//Displays dialogue on the screen
	public void DisplayDialogue(DialogueLine line)
	{
		if (!_dialogueCanvas.Visible)
		{
			_dialogueCanvas.Visible = true;
		}

		_nameLabel.Text = line.speakerName;
		_dialogueTextLabel.Text = line.dialogueText;
		_dialogueAnimation.AnimateNextDialogueLine(_dialogueTextLabel, line);
		_animatedSprite2D.Play(line.DialogueExpression.ToString());
		//_dialogueAnimation.SetLine(text);
	}

	public void DisplayResponses(string[] responses)
	{
		var buttons = new List<Button>();
		foreach (var response in responses)
		{
			Button button;
			_responseContainer.CallDeferred(Node.MethodName.AddChild, button = new Button());
			button.Text = response;
			button.ActionMode = Button.ActionModeEnum.Press;
			button.Pressed += () =>
			{
				_dialogueSystem.InsertSelectedResponse(response);
				ClearResponses();
			};
			buttons.Add(button);
		}

		buttons.First().CallDeferred(Button.MethodName.GrabFocus);
	}

	private void ClearResponses()
	{
		foreach (Node child in _responseContainer.GetChildren())
		{
			child.QueueFree();
		}
	}

	public void ClearDialogue()
	{
		_nameLabel.Text = "";
		_dialogueTextLabel.Text = "";
		_animatedSprite2D.Stop();
		_dialogueCanvas.Visible = false;
	}

	public void ShowAllDialogue()
	{
		_dialogueAnimation.CurrentLetterIndex = -1;
	}
}
