using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Events;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.NPC;

public partial class Bed : Area2D
{
	[Export] private DialogueResourceObject _dialogueResourceObject;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player.Player)
		{
			EventBus.Instance.InvokeStartingDialogue(_dialogueResourceObject);
		}
	}
}
