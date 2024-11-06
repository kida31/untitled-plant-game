using Godot;
using System;
using untitledplantgame.Dialogue;
using untitledplantgame.MagicBoxForData;

public partial class DialogueCanvas : CanvasLayer
{
	private RichTextLabel _nameLabel;
	private RichTextLabel _dialogueTextLabel;
	
	private ResourceManager _resourceManager;
	private DialogueResourceObject _currentDialogue;
	private int _currentDialogueIndex;
	public override void _Ready()
	{
		_nameLabel = GetNode<RichTextLabel>("NameBox/Name");
		_dialogueTextLabel = GetNode<RichTextLabel>("DialogueBox/DialogueText");

		_currentDialogue = LoadDialogueResource(0);
	}
	
	private DialogueResourceObject LoadDialogueResource(int id)
	{
		// Load dialogue from file
		_currentDialogueIndex = 0;
		return null;
	}

	private void DisplayDialogue(DialogueLine dialogueLine)
	{
		_nameLabel.Text = dialogueLine.speakerName;
		_dialogueTextLabel.Text = dialogueLine.dialogueText;
	}
	
	private void OnDialogueButtonPressed()
	{
		_currentDialogueIndex++;
		if (_currentDialogueIndex < _currentDialogue._dialogueText.Length)
		{
			DisplayDialogue(_currentDialogue._dialogueText[_currentDialogueIndex]);
		}
		else
		{
			// End dialogue
		}
	}
}
