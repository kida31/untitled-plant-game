// using System.Text.RegularExpressions;
using Godot;
using untitledplantgame.Common;

public abstract partial class AbstractNPC : Area2D, IInteractable
{
	[Export]
	public string ActionName { get; private set; } = "interact";

	[Export]
	private string _npcName;

	public override void _Ready()
	{
		AddToGroup(Group.Interactables);
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	public Vector2 GetGlobalInteractablePosition()
	{
		return GlobalPosition;
	}

	public abstract void Interact();

	protected virtual void OnBodyExited(Node body)
	{
		InteractionManager.Instance.UnregisterArea(this);
	}

	private void OnBodyEntered(Node body)
	{
		InteractionManager.Instance.RegisterArea(this);
	}
}
