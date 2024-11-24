using Godot;
using DialogueResourceObject = untitledplantgame.Dialogue.Models.DialogueResourceObject;
using DialogueSystem = untitledplantgame.Dialogue.DialogueSystem;

namespace untitledplantgame.Scenes.TestScenes.Dialogue;

public partial class TestDialogue : Node2D
{
	[Export]
	private DialogueResourceObject _exampleDialogue;

	[Export]
	private DialogueResourceObject _followupDialogue;

	public override void _Ready()
	{
		EventBus.Instance.InvokeStartingDialogue(_exampleDialogue._dialogueId);
	}
}
