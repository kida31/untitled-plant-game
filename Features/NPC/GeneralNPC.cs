using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;


namespace untitledplantgame.NPC;

public partial class GeneralNPC : AInteractable
{
	[Export]
	private string Name { get; set; }
	[Export] 
	private string NPCDescription { get; set; }
	[Export]
	private AnimatedSprite2D _characterPortrait;
	[Export]
	private AnimatedSprite2D _spriteSheet;
	[Export] 
	private string[] _dialogueIdSet;
	
	
	private void _Ready()
	{
		base._Ready();
	}

	public override void Interact()
	{
		runDialogue();
	}

	private void runDialogue()
	{
		EventBus.Instance.InvokeStartingDialogue(_dialogueIdSet[0]);
	}
}
