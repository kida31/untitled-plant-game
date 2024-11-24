using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue;

public partial class DialogueUi : Control
{
	[Export] private DialogueSystem _dialogueSystem;
	
	private RichTextLabel _nameLabel;
	private RichTextLabel _dialogueTextLabel;
	private AnimatedSprite2D _animatedSprite2D;
	private BoxContainer _responseContainer;
	private DialogueAnimation _dialogueAnimation;
	private int _currentDialogueIndex;

	public bool AnimationIsPlaying => _dialogueAnimation.AnimationIsPlaying;

	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("Portrait");
		_nameLabel = GetNode<RichTextLabel>("PanelContainer2/MarginContainer/Name");
		_dialogueTextLabel = GetNode<RichTextLabel>("PanelContainer/MarginContainer/DialogueText");
		_responseContainer = GetNode<BoxContainer>("Responses");

		_dialogueAnimation = new DialogueAnimation();
		AddChild(_dialogueAnimation);
	}

	//Displays dialogue on the screen
	public void DisplayDialogue(DialogueLine line)
	{
		_nameLabel.Text = line.speakerName;
		_dialogueTextLabel.Text = line.dialogueText;
		_dialogueAnimation.AnimateNextDialogueLine(_dialogueTextLabel, line);
		_animatedSprite2D.Play(line.DialogueExpression.ToString());
		Visible = true;
	}

	public void DisplayResponses(string[] responses)
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

	public void ClearDialogue()
	{
		Visible = false;
	}

	public void ShowAllDialogue()
	{
		_dialogueAnimation.CurrentLetterIndex = -1;
	}
}
