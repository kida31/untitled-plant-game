using Godot;
using untitledplantgame.Common;

public abstract partial class AbstractNPC : Area2D, IInteractable
{
	private readonly Logger _logger = new("NPC");

	[Export]
	private NpcLogic _npcLogicNode;

	[Export]
	private string _npcName;

	[Export]
	protected string ActionName { get; set; } = "interact";

	public override void _Ready()
	{
		AddToGroup("Interactables");
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}

	private void OnBodyEntered(Node body)
	{
		InteractionManager.Instance.RegisterArea(this);
		_npcLogicNode.ManageNpcCollisionWithPlayer(body, _npcName);
	}

	protected void OnBodyExited(Node body)
	{
		InteractionManager.Instance.UnregisterArea(this);
		_npcLogicNode.ManageNpcCollisionWithPlayer(body, _npcName);
	}

	public Vector2 GetGlobalInteractablePosition()
	{
		return GlobalPosition;
	}

	public abstract void Interact();

	public string GetActionName()
	{
		return ActionName;
	}
}
