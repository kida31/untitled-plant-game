using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using DialogueResourceObject = untitledplantgame.Dialogue.Models.DialogueResourceObject;
using DialogueSystem = untitledplantgame.Dialogue.DialogueSystem;

namespace untitledplantgame.Scenes.TestScenes.Dialogue;

public partial class TutorialDialogue : Node2D
{
	[Export]
	private DialogueResourceObject _gottfriedBeforeResponse;
	
	[Export]
	private DialogueResourceObject _gottfriedAfterResponse;
	
	[Export]
	private DialogueResourceObject _gottfriedHarvesting;
	
	[Export]
	private DialogueResourceObject _gottfriedVendingMachine;
	
	[Export]
	private DialogueResourceObject _gottfriedSeedBoy;
	
	[Export]
	private DialogueResourceObject _gottfriedEnd;

	private Array<DialogueResourceObject> _dialogueParts; //input all dialogue parts, check if said already, load "next"
	
	
	public override void _Ready()
	{
		EventBus.Instance.InvokeStartingDialogue(_gottfriedBeforeResponse._dialogueId);

		
	}
	
	
}
