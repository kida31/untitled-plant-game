using Godot;

namespace untitledplantgame.Dialogue.Models;

[GlobalClass]
public partial class DialogueLine : Resource //DO NOT rename variables or the resource will break
{
	[Export]
	public string speakerName;

	[Export]
	public CompressedTexture2D DialogueExpression;

	[Export]
	public string dialogueText;
}
