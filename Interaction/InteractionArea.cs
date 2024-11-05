using Godot;

public partial class InteractionArea : Area2D, IInteractable
{
	[Signal]
	public delegate void InteractedEventHandler();

	[Export]
	private string ActionName { get; set; } = "interact";

	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Node2D body2D)
		{
			var interactionManager = GetNode<InteractionManager>("/root/Interaction/InteractionManager");
			if (interactionManager != null)
			{
				interactionManager.RegisterArea(this);
			}
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body is Node2D body2D)
		{
			var interactionManager = GetNode<InteractionManager>("/root/Interaction/InteractionManager");
			if (interactionManager != null)
			{
				interactionManager.UnregisterArea(this);
			}
		}
	}

	public void Interact()
	{
		EmitSignal(nameof(InteractedEventHandler));
		GD.Print("Interacted with area: " + ActionName);
	}

	public Vector2 GetGlobalInteractablePosition()
	{
		return GlobalPosition;
	}

	public string GetActionName()
	{
		return ActionName;
	}
}
