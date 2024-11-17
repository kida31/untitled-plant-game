using Godot;
using untitledplantgame.Dialogue;
using ResourceManager = untitledplantgame.ResourceData.ResourceManager;

public partial class DialogueCanvas : CanvasLayer
{
	private RichTextLabel _nameLabel;
	private RichTextLabel _dialogueTextLabel;
	private CanvasLayer _dialogueCanvas;

	private DialogueResourceObject _currentDialogue;
	private int _currentDialogueIndex;

	public override void _Ready()
	{
		_dialogueCanvas = GetNode<CanvasLayer>(".");
		_nameLabel = GetNode<RichTextLabel>("PanelContainer2/MarginContainer/Name");
		_dialogueTextLabel = GetNode<RichTextLabel>("PanelContainer/MarginContainer/DialogueText");

		_currentDialogue = DialogueDatabase.Instance?.GetResourceById(0);
		_dialogueCanvas.Visible = true;
		_currentDialogueIndex = 0;
		DialogueSystem.Instance.OnDialogueEnd += (_) => GD.Print("Hello");
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
			_dialogueCanvas.Visible = false;
			// End dialogue
		}
	}
}
