using Godot;
using untitledplantgame.NPC;
using untitledplantgame.Player;

namespace untitledplantgame.ProximityCollision;

/// <summary>
///  Proximity trigger which allows checking for collisions with other npcs.
/// </summary>
public partial class NpcProximityTrigger : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnEnterArea2D;
	}

	private void OnEnterArea2D(Node2D node2D)
	{
		if (node2D is Npc npc)
		{
			CollisionManager.Instance.HandleNpcCollision(npc);
		}
	}
}
