using Godot;
using Godot.Collections;

namespace untitledplantgame.NPC;

public partial class NpcHandler : Node
{
	[Export] private Npc _npc;
	[Export] private Array<Node> _routines; //[Export] private List<Node> _checkpoints; each routine should have those I guess (if it needs them)
	
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
