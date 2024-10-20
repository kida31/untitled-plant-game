using Godot;
using untitledplantgame.Player;

public partial class NpcLogic : Node 
{
	[Export] private NPC _npcSelf;
	
	private CollisionManager _collisionManager;
	
	public override void _Ready()
	{
		_collisionManager = GetNode<CollisionManager>("../../CollisionManager"); // Reference to the global collision manager
	}

	public void ManageNpcCollisionWithPlayer(Node body, string npcName)
	{
		if (body is Player)
		{
			_collisionManager.HandleNpcCollision(npcName);
		}
	}
	
	//------------------------------------------------------------------------------------//
	
	public void InteractionLogic()
	{
		// Logic for talking to the NPC
		GD.Print("Talking to NPC");
	}
}
