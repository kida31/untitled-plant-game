using Godot;
using untitledplantgame.Common;

public abstract partial class AbstractNPC : Area2D, IInteractable
{
	[Export]
	public string ActionName { get; private set; } = "interact";

	[Export]
	private NpcLogic _npcLogicNode;

	[Export]
	private string _npcName;

	public override void _Ready()
	{
		AddToGroup("Interactables");
		BodyEntered += OnBodyEntered;
		// BodyExited += OnBodyExited; // Activate this once the Player is unable to move in shop
		Connect("body_exited", new Callable(this, nameof(OnBodyExited))); // Deactivate this once the Player is unable to move in shop
	}

	public Vector2 GetGlobalInteractablePosition()
	{
		return GlobalPosition;
	}

	public abstract void Interact();

	protected void OnBodyExited(Node body)
	{
		InteractionManager.Instance.UnregisterArea(this);
		_npcLogicNode.ManageNpcCollisionWithPlayer(body, _npcName);
	}

	private void OnBodyEntered(Node body)
	{
		InteractionManager.Instance.RegisterArea(this);
		_npcLogicNode.ManageNpcCollisionWithPlayer(body, _npcName);
	}
}
