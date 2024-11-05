using Godot;

public interface IInteractable
{
	public void Interact();
	public Vector2 GetGlobalInteractablePosition();

	public string GetActionName();
}
