using Godot;

public partial class EventBus : Node
{
	[Signal] public delegate void NPCInteractedEventHandler(Node npc);

	public void NotifyNPCInteracted(Node npc)
	{
		EmitSignal(nameof(NPCInteracted), npc);
	}
}
