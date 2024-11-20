using Godot;

namespace untitledplantgame.Features.Dialogue.Models;

[GlobalClass]
public partial class DialogueResourceObject : Resource
{
	[Export]
	public string _dialogueId { get; set; }

	[Export]
	public DialogueLine[] _dialogueText { get; set; }

	[Export]
	public DialogueResponse[] _responses { get; set; }
}
