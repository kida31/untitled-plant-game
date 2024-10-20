using Godot;

public partial class InteractableItem : Area2D, IInteractable
{
	public override void _Ready()
	{
		AddToGroup("Interactables");
	}
	
	public void Interact()
	{
		// Logic for picking up the item
		GD.Print("Picking up item");
	}

	public Vector2 GetGlobalInteractablePosition()
	{
		return GlobalPosition;
	}
}
