using Godot;
using System;
using untitledplantgame.Common.GameState;
using untitledplantgame.Dialogue;

public partial class TestDialogue : Node2D
{
	[Export] private DialogueResourceObject _exampleDialogue;
	public override void _Ready()
	{
		GD.Print($"Current game state: {GameStateMachine.Instance.CurrentState}");
		GD.Print("Trigger dialogue.");
		
		DialogueSystem.Instance.StartDialog(_exampleDialogue);
		
		GD.Print($"Current game state: {GameStateMachine.Instance.CurrentState}");
	}
}
