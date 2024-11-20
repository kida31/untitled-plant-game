using Godot;

public interface IInteractable
{
	string ActionName { get; }
	public void Interact();
	public Vector2 GetGlobalInteractablePosition();
}
