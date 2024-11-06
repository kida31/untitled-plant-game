using Godot;

namespace untitledplantgame.Dialogue;

[GlobalClass]
public partial class DialogueResourceObject : Resource
{
	[Export]
	public string _dialogueId;
	
	[Export]
	public DialogueLine[] _dialogueText;

	public DialogueResourceObject(string dialogueId, DialogueLine[] dialogueText)
	{
		_dialogueId = dialogueId;
		_dialogueText = dialogueText;
	}
}

