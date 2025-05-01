using Godot;

public interface IInteractable
{
	public string GetActionName();
	public void Interact();
	public Vector2 GetGlobalInteractablePosition();
}
