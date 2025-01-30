using Godot;
using untitledplantgame.NPC;
using untitledplantgame.Player;

namespace untitledplantgame.EasterEgg;

public partial class CollisionEasterEgg : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnEnter;
	}

	private void OnEnter(Node2D node2D)
	{
		if (node2D is Npc npc)
		{
			CollisionManager.Instance.HandleNpcCollision(npc);
		}
	}
}
