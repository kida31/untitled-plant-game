using Godot;

namespace untitledplantgame.Features.Dialogue.Models;

[GlobalClass]
public partial class DialogueLine : Resource
{
	[Export]
	public string speakerName;

	[Export]
	public DialogueExpression DialogueExpression;

	[Export]
	public string dialogueText;
}
