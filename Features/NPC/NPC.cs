using System;
// using System.Text.RegularExpressions;
using Godot;
using untitledplantgame.Common;

[Obsolete]
public partial class NPC : Area2D, IInteractable
{
	[Export]
	private string _npcName;

	[Export]
	private NpcLogic _npcLogicNode;
	public string ActionName { get; private set; } = "talk";

	public override void _Ready()
	{
		AddToGroup(Group.Interactables);
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}

	private void OnBodyEntered(Node body)
	{
		_npcLogicNode.ManageNpcCollisionWithPlayer(body, _npcName);
	}

	private void OnBodyExited(Node body)
	{
		return;
	}

	public Vector2 GetGlobalInteractablePosition()
	{
		return GlobalPosition;
	}

	public void Interact()
	{
		_npcLogicNode.InteractionLogic();
	}
}
