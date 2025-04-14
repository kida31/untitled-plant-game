using Godot;

namespace untitledplantgame.Dialogue.Models;

/// <summary>
/// This class can be the starting point for a conversation.
/// It represents the major block of spoken text.
/// </summary>
[GlobalClass]
public partial class DialogueResourceObject : Resource //DO NOT rename variables or the resource will break
{
	[Export] public string _dialogueId { get; set; }

	[Export] public DialogueLine[] _dialogueText { get; set; }

	[Export] public DialogueResponse[] _responses { get; set; }
}
