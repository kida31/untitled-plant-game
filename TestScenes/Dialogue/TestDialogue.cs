using Godot;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Dialogue;

public partial class TestDialogue : Node2D
{
	[Export]
	private DialogueResourceObject _exampleDialogue;

	[Export]
	private DialogueResourceObject _followupDialogue;

	public override void _Ready()
	{
		// Prepare


		GD.Print($"Current game state: {GameStateMachine.Instance.CurrentState}");
		GD.Print("Trigger dialogue.");

		DialogueSystem.Instance.StartDialog(_exampleDialogue._dialogueId);

		GD.Print($"Current game state: {GameStateMachine.Instance.CurrentState}");
	}
}
