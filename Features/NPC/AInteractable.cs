using Godot;
using untitledplantgame.Common;
using untitledplantgame.Interaction;

// Alternative name AAreaInteractable, to emphasize that it is using an Area
/// <summary>
/// Base class for interactable objects in the game.
/// It is an Area2D that can be interacted with.
/// <remarks>
/// The object registers itself to the InteractionManager when a body enters the area.
/// </remarks>
/// </summary>
public abstract partial class AInteractable : Area2D, IInteractable
{
	public virtual string ActionName => "Interact";

	public override void _Ready()
	{
		AddToGroup(GameGroup.Interactables);
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	public Vector2 GetGlobalInteractablePosition()
	{
		return GlobalPosition;
	}

	public abstract void Interact();
	
	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("player"))
		{
			InteractionManager.Instance.RegisterArea(this);
		}
	}
	
	protected virtual void OnBodyExited(Node body)
	{
		if (body.IsInGroup("player"))
		{
			InteractionManager.Instance.UnregisterArea(this);
		}
	}
}
