using System;
using Godot;
[Obsolote]
public partial class NPC : Area2D, IInteractable
{
	[Export]
	private string _npcName;

	[Export]
	private NpcLogic _npcLogicNode;
	private string ActionName { get; set; } = "talk";

	public override void _Ready()
	{
		AddToGroup("Interactables");
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
	
	public string GetActionName()
	{
		return ActionName;
	}
}

internal class ObsoloteAttribute : Attribute
{
}